using System.Threading;
using System.Threading.Tasks;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Interfaces
{
    public interface IUpsertProductDataService
    {
        Task<Result<Product>> UpsertProductAsync(UpsertProductRequest request, CancellationToken cancellationToken);
    }
}