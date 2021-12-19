using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SagaImpl.Common.MediatR
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validator)
        {
            this.validators = validator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var errors = GetErrors(request, cancellationToken);

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            return await next();
        }

        private List<ValidationFailure> GetErrors(TRequest request, CancellationToken cancellationToken)
        {
            var errors = validators.Select(async x => await x.ValidateAsync(request, cancellationToken))
                                   .SelectMany(x => x.Result.Errors)
                                   .Where(x => x != null)
                                   .ToList();

            return errors;
        }
    }
}
