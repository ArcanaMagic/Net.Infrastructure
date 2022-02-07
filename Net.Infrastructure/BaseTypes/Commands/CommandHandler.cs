using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Net.Infrastructure.BaseTypes.Commands
{
    /// <summary>
    /// Хендлер для перехвата любой команды, где вся логика реализуется в HandleAsync
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandHandler<TCommand, TRequest, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : MediatR.IRequest<TResponse> 
        where TRequest : Models.IRequest
        where TResponse : Models.IResponse
    {
        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            return await HandleAsync(command, cancellationToken);
        }

        protected abstract Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}
