using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net.Infrastructure.BaseTypes.Actions
{
    /// <summary>
    /// Контракт метода Get, для получения всех данных сущности
    /// </summary>
    public interface IGetAll<in TRequest, TResponse>
    {
        /// <summary>
        /// Параметрами из строки принимает значения из типа <typeparamref name="TRequest"/>, тело обычно пустое.<br/> Возвращает имплементацию <typeparamref name="TResponse"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        Task<ActionResult<TResponse>> GetAllAsync([FromQuery] TRequest request);
    }



}
