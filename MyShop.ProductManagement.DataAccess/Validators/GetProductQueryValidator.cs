using FluentValidation;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.DataAccess.Validators
{
    public class GetProductQueryValidator : ModelValidatorBase<GetProductQuery>
    {
        public GetProductQueryValidator()
        {
            // This is a data access level validation
            RuleFor(x => x.ProductCode).NotNull().NotEmpty();
        }
    }
}