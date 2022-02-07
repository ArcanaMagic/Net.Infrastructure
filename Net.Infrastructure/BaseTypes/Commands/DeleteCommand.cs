using System;
using MediatR;

namespace Net.Infrastructure.BaseTypes.Commands
{
    public abstract class DeleteCommand<TRequest, TResponse> : IRequest<TResponse> 
        where TRequest : Models.IRequest
        where TResponse : Models.IResponse
    {
        public Guid EntityId;
        public Type EntityType;
        public readonly string CorrelationId;
        public readonly string EmployeeId;

        protected DeleteCommand(Guid entityId, Type entityType, string correlationId, string employeeId)
        {
            EntityId = entityId;
            EntityType = entityType;
            CorrelationId = correlationId;
            EmployeeId = employeeId;
        }
    }
}