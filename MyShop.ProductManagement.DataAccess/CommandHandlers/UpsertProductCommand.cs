using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.DataAccess.CommandHandlers
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