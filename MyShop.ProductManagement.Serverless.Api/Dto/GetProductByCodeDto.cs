using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Serverless.Api.Dto
{
    public class GetProductByCodeDto : IDto, IRequest<Result<Product>>, IValidatableRequest
    {   
        public string CorrelationId { get; set; }
        public string ProductCode { get; set; }
    }
}