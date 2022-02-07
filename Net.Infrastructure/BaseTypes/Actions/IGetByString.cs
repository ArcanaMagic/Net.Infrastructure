using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net.Infrastructure.BaseTypes.Actions
{
    /// <summary>
    /// Контракт метода Get, для получения данных по текстовому полю
    /// </summary>
    public interface IGetByString<TResponse>
    {
        /// <summary>
        /// Параметром из роута принимает <b>text</b> типа <see cref="string"/>, тело пустое.<br/> Возвращает имплементацию <typeparamref name="TResponse"/>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet("{text}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        Task<ActionResult<TResponse>> GetByStringAsync([FromRoute] string text);
    }
}
