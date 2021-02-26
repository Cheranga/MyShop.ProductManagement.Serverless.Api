﻿using FluentValidation;
using MyShop.ProductManagement.Domain.Validators;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.Validators
{
    public class UpsertProductDtoValidator : ModelValidatorBase<UpsertProductDto>
    {
        public UpsertProductDtoValidator()
        {
            RuleFor(x => x.CorrelationId).NotNull().NotEmpty();
            RuleFor(x => x.ProductCode).NotNull().NotEmpty();
            RuleFor(x => x.ProductDescription).NotNull().NotEmpty();
        }
    }
}