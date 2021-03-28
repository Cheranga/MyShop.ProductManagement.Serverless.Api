using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Application.Messages;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Constants;
using ErrorCodes = MyShop.ProductManagement.Application.Constants.ErrorCodes;

namespace MyShop.ProductManagement.Application.Handlers
{
    internal class UpsertProductRequestHandler : IRequestHandler<UpsertProductRequest, Result<GetProductResponse>>
    {
        private readonly IMediator _mediator;

        public UpsertProductRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<GetProductResponse>> Handle(UpsertProductRequest request, CancellationToken cancellationToken)
        {
            var getProductQuery = new GetProductByCodeQuery(request.CorrelationId, request.ProductCode);
            var getProductOperation = await _mediator.Send(getProductQuery, cancellationToken);

            if (!getProductOperation.Status)
            {
                return Result<GetProductResponse>.Failure(getProductOperation.ErrorCode, getProductOperation.Validation);
            }

            var product = getProductOperation.Data;
            if (product == null)
            {
                return await InsertProductAsync(request, cancellationToken);
            }

            return await UpdateProductAsync(request, cancellationToken);
        }

        private async Task<Result<GetProductResponse>> UpdateProductAsync(UpsertProductRequest request, CancellationToken cancellationToken)
        {
            var updateProductMessage = new UpdateProductMessage
            {
                CorrelationId = request.CorrelationId,
                ProductCode = request.ProductCode,
                ProductName = request.ProductName
            };

            var operation = await _mediator.Send(updateProductMessage, cancellationToken);
            if (operation.Status)
            {
                return Result<GetProductResponse>.Success(new GetProductResponse
                {
                    ProductCode = request.ProductCode,
                    ProductName = request.ProductName
                });
            }

            return Result<GetProductResponse>.Failure(operation.ErrorCode, operation.Validation);
        }

        private async Task<Result<GetProductResponse>> InsertProductAsync(UpsertProductRequest request, CancellationToken cancellationToken)
        {
            var insertCommand = new InsertProductCommand(request.CorrelationId, request.ProductCode, request.ProductName);
            var operation = await _mediator.Send(insertCommand, cancellationToken);

            if (!operation.Status)
            {
                return Result<GetProductResponse>.Failure(operation.ErrorCode, operation.Validation);
            }

            var product = operation.Data;
            var getProductResponse = new GetProductResponse
            {
                ProductCode = product.ProductCode,
                ProductName = product.ProductName
            };

            return Result<GetProductResponse>.Success(getProductResponse);
        }
    }
}