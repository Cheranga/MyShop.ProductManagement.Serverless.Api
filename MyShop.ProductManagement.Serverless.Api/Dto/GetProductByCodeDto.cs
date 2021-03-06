﻿using MediatR;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Serverless.Api.Dto
{
    public class GetProductByCodeDto : IDto, IRequest<Result<GetProductResponse>>, IValidatableRequest
    {
        public string ProductCode { get; set; }
        public string CorrelationId { get; set; }
    }
}