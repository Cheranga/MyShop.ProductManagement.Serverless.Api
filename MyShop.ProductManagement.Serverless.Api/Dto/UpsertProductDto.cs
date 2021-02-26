using System;
using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Serverless.Api.Dto
{
    public class UpsertProductDto : IDto, IRequest<Result<Product>>, IValidatableRequest
    {
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N");
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
    }
}