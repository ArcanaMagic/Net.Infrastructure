using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net.Infrastructure.BaseTypes.Actions
{
    /// <summary>
    /// Контракт метода Get, для получения данных по реквесту
    /// </summary>
    public interface IGetByRequest<in TRequest, TResponse>
    {
        /// <summary>
        /// Возвращает имплементацию <typeparamref name="TResponse"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet()]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        Task<ActionResult<TResponse>> GetByRequestAsync([FromQuery] TRequest request);
    }
}
