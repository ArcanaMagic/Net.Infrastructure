using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Net.Infrastructure.Exceptions;

namespace Net.Infrastructure.ErrorsHandling
{
    /// <summary>
    /// Позволяет при ошибке валидации реквеста формировать ValidateException
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ApiValidator<T> : AbstractValidator<T>
    {
        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = new CancellationToken())
        {
            var result = await base.ValidateAsync(context, cancellation);

            GenerateValidateException(result);

            return result;
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var result = base.Validate(context);

            GenerateValidateException(result);

            return result;
        }
        /// <summary>
        /// Получаем список ошибок валидации от FluentValidator и сериализуем их в текстовую строку для возбуждения ValidateException
        /// </summary>
        /// <param name="result"></param>
        private void GenerateValidateException(ValidationResult result)
        {
            if (result.IsValid)
                return;

            var sb = new StringBuilder();

            var errors = result.Errors.Select(x => new { x.PropertyName, x.ErrorMessage}).ToList();

            for (var i = 0; i < errors.Count; i++)
            {
                sb.Append(errors[i].PropertyName + ": " + errors[i].ErrorMessage);

                if (errors.Count != i + 1)
                {
                    sb.Append("^");
                }
            }

            throw new ValidateException(sb.ToString());
        }
    }
}
