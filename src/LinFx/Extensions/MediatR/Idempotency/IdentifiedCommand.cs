using MediatR;
using System;

namespace LinFx.Extensions.Mediator.Idempotency
{
    public class IdentifiedCommand<TCommand, TResponse> : IRequest<TResponse> where TCommand : IRequest<TResponse>
    {
        public TCommand Command { get; }

        public Guid Id { get; }

        public IdentifiedCommand(TCommand command, Guid id)
        {
            Command = command;
            Id = id;
        }
    }
}
