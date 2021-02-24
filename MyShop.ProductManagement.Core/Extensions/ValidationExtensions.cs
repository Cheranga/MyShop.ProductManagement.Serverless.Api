using System.Linq;
using FluentValidation.Results;

namespace MyShop.ProductManagement.Domain.Extensions
{
    public static class ValidationExtensions
    {
        public static string[] ToErrorMessage(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
            {
                return new string[] { };
            }

            var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToArray();
            return errors;
        }
    }
}