using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Services
{
    internal class UpsertProductService : IUpsertProductService
    {
        private readonly IMediator _mediator;

        public UpsertProductService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<GetProductResponse>> UpsertProductAsync(UpsertProductRequest request)
        {
            var cancellationToken = new CancellationTokenSource().Token;

            var operation = await _mediator.Send(request, cancellationToken);

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