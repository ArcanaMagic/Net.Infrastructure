using System;
using MediatR;

namespace Net.Infrastructure.BaseTypes.Commands
{
    public abstract class CreateCommand<TRequest, TResponse> : IRequest<TResponse> 
        where TRequest : Models.IRequest
        where TResponse : Models.IResponse
    {
        public TRequest Request;
        public readonly string CorrelationId;
        public readonly string EmployeeId;

        protected CreateCommand(TRequest request, string correlationId = null, string employeeId = null)
        {
            Request = request;
            CorrelationId = correlationId;
            EmployeeId = employeeId;
        }
    }
}