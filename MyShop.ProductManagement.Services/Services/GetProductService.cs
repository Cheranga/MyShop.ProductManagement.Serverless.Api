using System.Threading;
using System.Threading.Tasks;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Services
{
    internal class GetProductService : IGetProductService
    {
        private readonly IGetProductDataService _dataService;

        public GetProductService(IGetProductDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Result<GetProductResponse>> GetProductAsync(GetProductByCodeRequest request)
        {
            //
            // TODO: Perform application validation here.
            //

            var cancellationToken = new CancellationTokenSource().Token;

            var operation = await _dataService.GetProductAsync(request, cancellationToken);

            if (!operation.Status)
            {
                return Result<GetProductResponse>.Failure(operation.Validation);
            }

            var product = operation.Data;

            if (product == null)
            {
                return Result<GetProductResponse>.Success(null);
            }

            var productResponse = new GetProductResponse
            {
                ProductCode = product.ProductCode,
                ProductName = product.ProductName
            };

            return Result<GetProductResponse>.Success(productResponse);
        }
    }
}