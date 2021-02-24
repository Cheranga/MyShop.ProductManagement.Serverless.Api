using FluentValidation;
using MyShop.ProductManagement.DataAccess.Queries;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.DataAccess.Validators
{
    public class GetProductQueryValidator : ModelValidatorBase<GetProductQuery>
    {
        public GetProductQueryValidator()
        {
            RuleFor(x => x.ProductCode).NotNull().NotEmpty();
        }
    }
}