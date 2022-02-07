using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net.Infrastructure.BaseTypes.Actions
{
    /// <summary>
    /// Контракт метода Delete, для удаления(деактивации) сущности
    /// </summary>
    public interface IDeleteByRequest<in TRequest, TResponse>
    {
        /// <summary>
        /// <br/><i><b>Note: </b>По логике CQRS, метод не должен возвращать данные</i>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        Task<ActionResult<TResponse>> DeleteByRequestAsync([FromBody] TRequest request);
    }
}
