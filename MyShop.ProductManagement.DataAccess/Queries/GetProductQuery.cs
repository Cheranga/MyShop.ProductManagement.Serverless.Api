using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.DataAccess.Queries
{
    public class GetProductQuery : IRequest<Result<Product>>, IValidatableRequest
    {
        public GetProductQuery(string productCode)
        {
            ProductCode = productCode;
        }

        public string ProductCode { get; }
    }
}