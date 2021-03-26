using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.DataAccess
{
    public class InsertProductCommand : IRequest<Result<Product>>, IValidatableRequest
    {
        public string ProductCode { get; }
        public string ProductName { get; }

        public InsertProductCommand(string productCode, string productName)
        {
            ProductCode = productCode;
            ProductName = productName;
        }
    }
}