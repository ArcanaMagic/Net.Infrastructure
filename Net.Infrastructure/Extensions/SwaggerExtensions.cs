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

        private static string _version;
        private static string _appName;
        private static string _description; 
        /// <summary>
        /// Добавление службы
        /// </summary>
        public static void AddSwaggerDocumentation(
            this IServiceCollection services,
            string version,
            string appName,
            string description)
        {
            _version = version;
            _appName = appName;
            _description = description;

            services.AddSwaggerGen(options =>
            {
                /*
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
                */
                options.SwaggerDoc(
                    version,
                    new OpenApiInfo
                    {
                        Version = version,
                        Title = appName,
                        Description = description
                    });

                options.IgnoreObsoleteActions();

                // Set the comments path for the Swagger JSON and UI.
                //foreach (var swaggerXmlDocFile in SwaggerXmlDocFiles)
                //    c.IncludeXmlComments(swaggerXmlDocFile);
            });
        } 

        /// <summary>
        /// Middleware для подключения настроек сваггера
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerDocumentation(
            this IApplicationBuilder app)
        {
            app.UseSwagger(/*options =>
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
            }*/);
            app.UseSwaggerUI(
                    options =>
                    {
                        options.SwaggerEndpoint($"/swagger/{_version}/swagger.json", $"{_appName} {_version}");
                        options.OAuthAppName(_appName);
                        //options.OAuthClientId(AuthOptions.ClientId);
                        //options.OAuthClientSecret(AuthOptions.ClientSecret);
                        options.DocExpansion(DocExpansion.None);
                    });
        }
    }
}
