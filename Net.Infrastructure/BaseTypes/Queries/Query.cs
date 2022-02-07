using System;
using MediatR;

namespace Net.Infrastructure.BaseTypes.Queries
{
    public abstract class Query<TRequest, TResponse> : IRequest<TResponse> 
        where TRequest : Models.IRequest
        where TResponse : Models.IResponse
    {
        public TRequest Request;

        protected Query(TRequest request)
        {
            Request = request;
        }

    }
}
