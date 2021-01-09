using System.Threading.Tasks;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.Services.Requests;
using MyShop.ProductManagement.Services.Responses;

namespace MyShop.ProductManagement.Api.Services
{
    public interface IProductsService
    {
        Task<Result<GetProductResponse>> UpsertProductAsync(UpsertProductRequest request);
        Task<Result<GetProductResponse>> GetProductAsync(GetProductRequest request);
    }
}