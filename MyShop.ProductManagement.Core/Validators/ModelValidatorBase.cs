﻿using FluentValidation;
using FluentValidation.Results;

namespace MyShop.ProductManagement.Domain.Validators
{
    public abstract class ModelValidatorBase<T> : AbstractValidator<T>
    {
        protected ModelValidatorBase()
        {
            CascadeMode = CascadeMode.Stop;
        }

        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure("", "Instance is null"));
                return false;
            }

            return true;
        }
    }
}