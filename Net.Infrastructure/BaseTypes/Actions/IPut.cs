using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net.Infrastructure.BaseTypes.Actions
{
    /// <summary>
    /// Контракт метода Put для изменения сущности
    /// </summary>
    public interface IPut<in TRequest, TResponse>
    {
        /// <summary>
        /// <br/><i><b>Note: </b>По логике CQRS, метод не должен возвращать данные</i>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        Task<ActionResult<TResponse>> PutAsync([FromRoute] Guid id, [FromBody] TRequest request);
    }
}
