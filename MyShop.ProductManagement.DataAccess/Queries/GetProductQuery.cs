using MediatR;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.Models;

namespace MyShop.ProductManagement.DataAccess.Queries
{
    public class GetProductQuery : IRequest<Result<Product>>
    {
        public GetProductQuery(string productCode)
        {
            ProductCode = productCode;
        }

        public string ProductCode { get; }
    }
}