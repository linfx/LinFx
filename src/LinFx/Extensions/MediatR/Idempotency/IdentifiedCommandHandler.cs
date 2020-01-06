using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.Mediator.Idempotency
{
    /// <summary>
    /// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
    /// a requestid sent by client is used to detect duplicate requests.
    /// </summary>
    /// <typeparam name="TCommand">Type of the command handler that performs the operation if request is not duplicated</typeparam>
    /// <typeparam name="TResponse">Return value of the inner command handler</typeparam>
    public class IdentifiedCommandHandler<TCommand, TResponse> : IRequestHandler<IdentifiedCommand<TCommand, TResponse>, TResponse> where TCommand : IRequest<TResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;

        public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager)
        {
            _mediator = mediator;
            _requestManager = requestManager;
        }

        /// <summary>
        /// Creates the result value to return if a previous request was found
        /// </summary>
        /// <returns></returns>
        protected virtual TResponse CreateResultForDuplicateRequest()
        {
            return default;
        }

        /// <summary>
        /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
        /// just enqueues the original inner command.
        /// </summary>
        /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
        /// <returns>Return value of inner command or default value if request same ID was found</returns>
        public async Task<TResponse> Handle(IdentifiedCommand<TCommand, TResponse> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync<TCommand>(message.Id);
                try
                {
                    // Send the embeded business command to mediator so it runs its related CommandHandler 
                    var result = await _mediator.Send(message.Command, cancellationToken);
                    return result;
                }
                catch
                {
                    return default;
                }
            }
        }
    }
}