using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.DataAccess
{
    public class UpsertProductCommand : IRequest<Result<Product>>, IValidatableRequest
    {
        public UpsertProductCommand(string productCode, string productName)
        {
            ProductCode = productCode;
            ProductName = productName;
        }

        public string ProductCode { get; }
        public string ProductName { get; }
    }
}