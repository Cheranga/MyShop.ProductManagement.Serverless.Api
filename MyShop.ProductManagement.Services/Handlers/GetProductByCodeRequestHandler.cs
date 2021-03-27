using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Handlers
{
    internal class GetProductByCodeRequestHandler : IRequestHandler<GetProductByCodeRequest, Result<GetProductResponse>>
    {
        private readonly IMediator _mediator;

        public GetProductByCodeRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<GetProductResponse>> Handle(GetProductByCodeRequest request, CancellationToken cancellationToken)
        {
            var query = new GetProductByCodeQuery(request.CorrelationId, request.ProductCode);
            var operation = await _mediator.Send(query, cancellationToken);

            if (!operation.Status)
            {
                return Result<GetProductResponse>.Failure(operation.ErrorCode, operation.Validation);
            }

            var product = operation.Data;
            if (product == null)
            {
                return Result<GetProductResponse>.Failure(ErrorCodes.ProductNotFound, "Product not found.");
            }

            var productResponse = new GetProductResponse
            {
                ProductCode = request.ProductCode,
                ProductName = product.ProductName
            };

            return Result<GetProductResponse>.Success(productResponse);
        }
    }
}