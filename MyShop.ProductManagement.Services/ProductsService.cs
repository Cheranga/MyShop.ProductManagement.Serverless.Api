using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Core;
using MyShop.ProductManagement.DataAccess.CommandHandlers;
using MyShop.ProductManagement.DataAccess.Queries;
using MyShop.ProductManagement.Services.Requests;
using MyShop.ProductManagement.Services.Responses;

namespace MyShop.ProductManagement.Api.Services
{
    internal class ProductsService : IProductsService
    {
        private readonly ILogger<ProductsService> _logger;
        private readonly IMediator _mediator;

        public ProductsService(IMediator mediator, ILogger<ProductsService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Result<GetProductResponse>> UpsertProductAsync(UpsertProductRequest request)
        {
            _logger.LogInformation("Upserting product {correlationId}", request.CorrelationId);

            var operation = await _mediator.Send(new UpsertProductCommand(request.ProductId, request.ProductCode, request.ProductName));
            if (!operation.Status)
            {
                _logger.LogError("{correlationId} Error occured when upserting product. {upsertProductRequest}", request.CorrelationId, request);
                return Result<GetProductResponse>.Failure(operation.Validation);
            }

            _logger.LogInformation("{correlationId} Upserting product successful. {upsertProductRequest}", request.CorrelationId, request);

            var response = new GetProductResponse
            {
                ProductCode = operation.Data.ProductCode,
                ProductName = operation.Data.ProductName
            };

            return Result<GetProductResponse>.Success(response);
        }

        public async Task<Result<GetProductResponse>> GetProductAsync(GetProductRequest request)
        {
            _logger.LogInformation("Getting product {correlationId}", request.CorrelationId);

            var operation = await _mediator.Send(new GetProductQuery(request.ProductCode));
            if (!operation.Status)
            {
                _logger.LogError("{correlationId} error occured when getting the product.", request.CorrelationId);
                return Result<GetProductResponse>.Failure("", "Error occured when getting the product.");
            }

            var product = operation.Data;

            if (product == null)
            {
                return Result<GetProductResponse>.Failure("", "Product not found.");
            }

            var response = new GetProductResponse
            {
                ProductCode = product.ProductCode,
                ProductName = product.ProductName
            };

            return Result<GetProductResponse>.Success(response);
        }
    }
}