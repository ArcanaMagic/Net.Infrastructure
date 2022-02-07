using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Net.Infrastructure.BaseTypes.Actions
{
    /// <summary>
    /// Контракт метода Get, для получения данных по Id сущности
    /// </summary>
    public interface IGetByGuid<TResponse>
    {
        /// <summary>
        /// Параметром из роута принимает <b>id</b> типа <see cref="Guid"/>, тело пустое.<br/> Возвращает имплементацию <typeparamref name="TResponse"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
        Task<ActionResult<TResponse>> GetByGuidAsync([FromRoute] Guid id);
    }
}
