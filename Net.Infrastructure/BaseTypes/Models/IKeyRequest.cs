using System;

namespace Net.Infrastructure.BaseTypes.Models
{
    /// <summary>
    /// Интерфейс для модели данных содержащей только Id
    /// </summary>
    public interface IKeyRequest : IRequest
    {
        Guid Id { get; set; }
    }
}
