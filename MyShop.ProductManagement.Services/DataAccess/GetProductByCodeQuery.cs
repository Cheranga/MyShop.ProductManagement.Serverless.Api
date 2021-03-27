using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.DataAccess
{
    public class GetProductByCodeQuery : IRequest<Result<Product>>, IValidatableRequest
    {
        public GetProductByCodeQuery(string correlationId, string productCode)
        {
            CorrelationId = correlationId;
            ProductCode = productCode;
        }

        public string ProductCode { get; }

        public string CorrelationId { get; }
    }
}