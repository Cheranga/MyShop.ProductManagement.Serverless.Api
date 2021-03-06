﻿using FluentValidation;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Validators
{
    public class GetProductByCodeRequestValidator : ModelValidatorBase<GetProductByCodeRequest>
    {
        public GetProductByCodeRequestValidator()
        {
            RuleFor(x => x.CorrelationId).NotNull().NotEmpty();

            // This is a application level validation
            RuleFor(x => x.ProductCode).NotNull().NotEmpty().Length(8);
        }
    }
}