using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Domain.Constants;
using MyShop.ProductManagement.Domain.Extensions;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Domain.Behaviours
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
            if (_validator == null)
            {
                return await next();
            }

            _logger.LogInformation("Validating {request}", typeof(TRequest).Name);

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation error: {errors}", validationResult.ToErrorMessage());
                return Result<TResponse>.Failure(ErrorCodes.ValidationError, validationResult);
            }

            var operation = await next();
            return operation;
        }
    }
}