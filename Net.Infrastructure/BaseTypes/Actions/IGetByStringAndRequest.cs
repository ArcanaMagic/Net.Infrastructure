using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net.Infrastructure.BaseTypes.Actions
{
    /// <summary>
    /// Контракт метода Get, для получения данных по текстовому полю
    /// </summary>
    public interface IGetByStringAndRequest<in TRequest, TResponse>
    {
        /// <summary>
        /// Параметром из роута принимает <b>text</b> типа <see cref="string"/>, тело пустое.<br/> Возвращает имплементацию <typeparamref name="TResponse"/>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{text}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        Task<ActionResult<TResponse>> GetByStringAndRequestAsync([FromRoute] string text, [FromQuery] TRequest request);
    }
}
