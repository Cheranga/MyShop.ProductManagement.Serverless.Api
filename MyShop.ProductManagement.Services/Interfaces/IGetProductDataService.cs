using System.Threading;
using System.Threading.Tasks;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Interfaces
{
    public interface IGetProductDataService
    {
        Task<Result<Product>> GetProductAsync(GetProductByCodeRequest request, CancellationToken cancellationToken);
    }
}