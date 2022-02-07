using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

// ReSharper disable UnusedMember.Global

namespace Net.Infrastructure.Extensions
{
    public static class SwaggerExtensions
    {
        public static string AppName { get; set; }
        public static string Version { get; set; } = "v1";
        public static string[] Scopes { get; set; } = null;
        public static Dictionary<string, string> DictionaryScopes { get; set; }

        //private static readonly IEnumerable<string> SwaggerXmlDocFiles = new List<string> { $"{Assembly.GetExecutingAssembly().GetName().Name}.xml" };

        /// <summary>
        /// For use auth service with swagger.
        /// Mark Controllers/Actions in swagger UI with Auth button
        /// </summary>
        private class SecurityRequirementsOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var found = false;
                if (context.ApiDescription.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controller)
                    found = controller.ControllerTypeInfo.GetCustomAttributes().OfType<AuthorizeAttribute>().Any();

                if (!found)
                {
                    if (!context.ApiDescription.TryGetMethodInfo(out var mi))
                        return;

                    if (!mi.GetCustomAttributes().OfType<AuthorizeAttribute>().Any())
                        return;
                }

                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                },
                                UnresolvedReference = true
                            },
                            new List<string>()
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Добавление службы
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appName"></param>
        /// <param name="options"></param>
        public static IServiceCollection AddSwagger(this IServiceCollection services, string currentHostUrl, Action<SwaggerGenOptions> options = null)
        {
            var swaggerApplicationDescription = $"Api for information by {AppName}";

            var authorizationUrl = (currentHostUrl + "/account/sign-in-by-credential").ToUri();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Please insert authorization token",
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.SwaggerDoc(
                    Version,
                    new OpenApiInfo
                    {
                        Version = Version,
                        Title = AppName,
                        Description = swaggerApplicationDescription
                    });

                c.IgnoreObsoleteActions();

                // Set the comments path for the Swagger JSON and UI.
                //foreach (var swaggerXmlDocFile in SwaggerXmlDocFiles)
                //    c.IncludeXmlComments(swaggerXmlDocFile);
                
                options?.Invoke(c);
            });


            return services;
        }

        /// <summary>
        /// Middleware для подключения настроек сваггера
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger(options =>
            {

                // Принудительная замена BasePath урлов свагера на https для кубера
                options.PreSerializeFilters.Add((document, request) =>
                {
                    document.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"https://{request.Host.Value}" }
                    };

                    //foreach (var kvp in document.Paths)
                    //{
                        //var key = request.Headers["X-Forwarded-Path"] + kvp.Key;
                        //var value = kvp.Value;
                        //var headers = request.Headers.ToList();
                    //}
                });
            });
            app.UseSwaggerUI(
                    options =>
                    {
                        options.SwaggerEndpoint($"/swagger/{Version}/swagger.json", $"{AppName} {Version}");
                        options.OAuthAppName(AppName);
                        //options.OAuthClientId(AuthOptions.ClientId);
                        //options.OAuthClientSecret(AuthOptions.ClientSecret);
                        options.DocExpansion(DocExpansion.None);
                    });

            return app;
        }
    }
}
