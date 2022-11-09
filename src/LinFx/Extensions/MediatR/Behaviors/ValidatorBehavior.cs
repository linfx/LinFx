using FluentValidation;
using MediatR;

namespace LinFx.Extensions.MediatR.Behaviors;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //var context = new ValidationContext(request);
        //var failures = _validators
        //    .Select(v => v.Validate(context))
        //    .SelectMany(result => result.Errors)
        //    .Where(f => f != null)
        //    .ToList();

        //if (failures.Any())
        //    throw new UserFriendlyException($"Command Validation Errors for type {typeof(TRequest).Name}", new FluentValidation.ValidationException("Validation exception", failures));

        //return next();

        throw new NotImplementedException();
    }
}
