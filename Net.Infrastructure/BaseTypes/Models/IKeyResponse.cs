using System;

namespace Net.Infrastructure.BaseTypes.Models
{
    /// <summary>
    /// Интерфейс для возврата Id созданной записи
    /// </summary>
    public interface IKeyResponse : IResponse
    {
        Guid Id { get; set; }
    }
}
