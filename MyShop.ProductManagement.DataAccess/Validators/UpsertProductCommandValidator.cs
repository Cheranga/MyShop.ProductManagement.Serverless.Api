using FluentValidation;
using MyShop.ProductManagement.DataAccess.CommandHandlers;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.DataAccess.Validators
{
    public class UpsertProductCommandValidator : ModelValidatorBase<UpsertProductCommand>
    {
        public UpsertProductCommandValidator()
        {
            RuleFor(x => x.ProductName).NotNull().NotEmpty();
            RuleFor(x => x.ProductCode).NotNull().NotEmpty();
        }
    }
}