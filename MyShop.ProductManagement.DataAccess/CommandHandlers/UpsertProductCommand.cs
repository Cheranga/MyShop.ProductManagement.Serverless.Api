using MediatR;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.Models;

namespace MyShop.ProductManagement.DataAccess.CommandHandlers
{
    public class UpsertProductCommand : IRequest<Result<Product>>
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