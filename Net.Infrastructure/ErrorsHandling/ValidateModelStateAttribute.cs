using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Net.Infrastructure.Exceptions;

namespace Net.Infrastructure.ErrorsHandling
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Позволяет в случае ошибки валидации, формировать список ошибок в нужном виде и бросать ValidateException
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) 
                return;

            var values = context.ModelState.Values.ToList();

            var sb = new StringBuilder();

            for (var i = 0; i < values.Count; i++)
            {
                foreach (var t in values[i].Errors)
                {
                    sb.Append(t.ErrorMessage);

                    if (values.Count != i + 1)
                    {
                        sb.Append("^");
                    }
                }
            }

            throw new ValidateException(sb.ToString());
        }
    }
}
