using MediatR;
using System;

namespace LinFx.Extensions.MediatR.Idempotency
{
    public class IdentifiedCommand<TCommand, TResponse> : IRequest<TResponse> where TCommand : IRequest<TResponse>
    {
        public Guid Id { get; }

        public TCommand Command { get; }

        public IdentifiedCommand(Guid id, TCommand command)
        {
            Id = id;
            Command = command;
        }
    }
}
