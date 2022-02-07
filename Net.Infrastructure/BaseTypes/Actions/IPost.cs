using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net.Infrastructure.BaseTypes.Actions
{
    /// <summary>
    /// Контракт метода Post, для создания сущности
    /// </summary>
    public interface IPost<in TRequest, TResponse>
    {
        /// <summary>
        /// <br/><i><b>Note: </b>По логике CQRS, метод не должен возвращать данные</i>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        Task<ActionResult<TResponse>> PostAsync([FromBody] TRequest request);
    }
}
