using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.DataAccess
{
    public class UpdateProductCommand : IRequest<Result<Product>>, IValidatableRequest
    {
        public UpdateProductCommand(string correlationId, string productCode, string productName)
        {
            CorrelationId = correlationId;
            ProductCode = productCode;
            ProductName = productName;
        }

        public string ProductCode { get; }
        public string ProductName { get; }

        public string CorrelationId { get; }
    }
}