using System.Threading.Tasks;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Interfaces
{
    public interface IGetProductService
    {
        Task<Result<GetProductResponse>> GetProductAsync(GetProductByCodeRequest request);
    }
}