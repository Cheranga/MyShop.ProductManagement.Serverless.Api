using FluentValidation;
using MyShop.ProductManagement.Domain.Validators;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.Validators
{
    public class GetProductByCodeDtoValidator : ModelValidatorBase<GetProductByCodeDto>
    {
        public GetProductByCodeDtoValidator()
        {
            // This is an API level validation
            RuleFor(x => x.CorrelationId).NotNull().NotEmpty().MinimumLength(5);
            RuleFor(x => x.ProductCode).NotNull().NotEmpty();
        }
    }
}