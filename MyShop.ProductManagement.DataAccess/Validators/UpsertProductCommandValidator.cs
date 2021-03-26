using FluentValidation;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.DataAccess.Validators
{
    public class InsertProductCommandValidator : ModelValidatorBase<InsertProductCommand>
    {
        public InsertProductCommandValidator()
        {
            RuleFor(x => x.ProductName).NotNull().NotEmpty();
            RuleFor(x => x.ProductCode).NotNull().NotEmpty();
        }
    }
}