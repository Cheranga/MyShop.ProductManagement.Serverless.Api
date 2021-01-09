using MediatR;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.Models;

namespace MyShop.ProductManagement.DataAccess.CommandHandlers
{
    public class UpsertProductCommand : IRequest<Result<ProductDataModel>>
    {
        public UpsertProductCommand(int id, string productCode, string productName)
        {
            Id = id;
            ProductCode = productCode;
            ProductName = productName;
        }

        public int Id { get; }
        public string ProductCode { get; }
        public string ProductName { get; }
    }
}