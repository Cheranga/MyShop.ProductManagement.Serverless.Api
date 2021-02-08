using System.Threading;
using System.Threading.Tasks;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Services
{
    internal class UpsertProductService : IUpsertProductService
    {
        private readonly IUpsertProductDataService _dataService;

        public UpsertProductService(IUpsertProductDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Result<GetProductResponse>> UpsertProductAsync(UpsertProductRequest request)
        {
            //
            // TODO: Validation
            //

            var cancellationToken = new CancellationTokenSource().Token;
            var operation = await _dataService.UpsertProductAsync(request, cancellationToken);

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