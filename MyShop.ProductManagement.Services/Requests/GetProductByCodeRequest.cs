using MediatR;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Requests
{
    public class GetProductByCodeRequest : IRequest<Result<GetProductResponse>>, IValidatableRequest
    {
        public GetProductByCodeRequest(string correlationId, string productCode)
        {
            CorrelationId = correlationId;
            ProductCode = productCode;
        }

        public string ProductCode { get; }

        public string CorrelationId { get; }
    }
}