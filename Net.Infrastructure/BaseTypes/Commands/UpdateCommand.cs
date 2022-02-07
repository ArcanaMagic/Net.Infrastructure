using System;
using MediatR;

namespace Net.Infrastructure.BaseTypes.Commands
{
    public abstract class UpdateCommand<TRequest, TResponse> : IRequest<TResponse> 
        where TRequest : Models.IRequest
        where TResponse : Models.IResponse
    {
        public TRequest Request;
        public Guid EntityId;
        public readonly string CorrelationId;
        public readonly string EmployeeId;

        protected UpdateCommand(TRequest request, Guid entityId, string correlationId, string employeeId)
        {
            Request = request;
            CorrelationId = correlationId;
            EmployeeId = employeeId;
            EntityId = entityId;
        }
    }
}