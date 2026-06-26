using ErrorOr;
using FluentValidation;
using MediatR;

namespace Aplicacion.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var fallos = _validators
                .Select(validator => validator.Validate(context))
                .SelectMany(resultado => resultado.Errors)
                .Where(fallo => fallo is not null)
                .ToList();

            if (fallos.Count == 0)
            {
                return await next();
            }

            var errores = fallos
                .ConvertAll(fallo => Error.Validation(fallo.PropertyName, fallo.ErrorMessage));

            return (dynamic)errores;
        }
    }
}
