using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.DataAccess
{
    public class InsertProductCommand : IRequest<Result<Product>>, IValidatableRequest
    {
        public InsertProductCommand(string correlationId, string productCode, string productName)
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