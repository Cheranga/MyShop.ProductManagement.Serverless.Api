using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Domain.Extensions;

namespace MyShop.ProductManagement.Domain.Validators
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>> where TRequest : IValidatableRequest
    {
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;
        private readonly IValidator<TRequest> _validator;

        public ValidationBehaviour(IValidator<TRequest> validator, ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
        {
            _validator = validator;
            _logger = logger;
        }

        public async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Result<TResponse>> next)
        {
            _logger.LogInformation("Validating {request}", typeof(TRequest).Name);

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation error: {errors}", validationResult.ToErrorMessage());
                return Result<TResponse>.Failure(validationResult);
            }

            var operation = await next();
            return operation;
        }
    }
}