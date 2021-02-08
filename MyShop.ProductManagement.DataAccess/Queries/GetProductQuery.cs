using MediatR;
using MyShop.ProductManagement.Domain;

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