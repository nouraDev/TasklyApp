using FluentValidation;
using MediatR;
using TaskApp.Application.Common;

namespace TaskApp.Application.ValidationBehavior
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count > 0)
                {
                    var error = Error.ValidationFailed(failures.Select(f => f.ErrorMessage));

                    var responseType = typeof(TResponse);
                    if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Response<>))
                    {
                        var method = responseType.GetMethod(nameof(Response<object>.ValidationFailure));
                        return (TResponse)method!.Invoke(null, new object[] { error })!;
                    }

                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }

}

