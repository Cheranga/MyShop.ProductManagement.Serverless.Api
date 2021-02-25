﻿using FluentValidation;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Validators
{
    public class UpsertProductRequestValidator : ModelValidatorBase<UpsertProductRequest>
    {
        public UpsertProductRequestValidator()
        {
            RuleFor(x => x.CorrelationId).NotNull().NotEmpty();
            RuleFor(x => x.ProductCode).NotNull().NotEmpty().Length(10);
            RuleFor(x => x.ProductName).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}