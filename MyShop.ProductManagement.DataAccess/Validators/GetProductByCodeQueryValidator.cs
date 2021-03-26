using FluentValidation;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.DataAccess.Validators
{
    public class GetProductByCodeQueryValidator : ModelValidatorBase<GetProductByCodeQuery>
    {
        public GetProductByCodeQueryValidator()
        {
            RuleFor(x => x.CorrelationId).NotNull().NotEmpty();
            // This is a data access level validation
            RuleFor(x => x.ProductCode).NotNull().NotEmpty();
        }
    }
}