using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Handlers
{
    internal class UpsertProductRequestHandler : IRequestHandler<UpsertProductRequest, Result<Product>>
    {
        private readonly IMediator _mediator;

        public UpsertProductRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> Handle(UpsertProductRequest request, CancellationToken cancellationToken)
        {
            var getProductQuery = new GetProductByCodeQuery(request.CorrelationId, request.ProductCode);
            var getProductOperation = await _mediator.Send(getProductQuery, cancellationToken);

            if (!getProductOperation.Status)
            {
                return Result<Product>.Failure(getProductOperation.Validation);
            }

            var product = getProductOperation.Data;
            if (product == null)
            {
                return await InsertProductAsync(request, cancellationToken);
            }

            return await UpdateProductAsync(request, cancellationToken);
        }

        private async Task<Result<Product>> UpdateProductAsync(UpsertProductRequest request, CancellationToken cancellationToken)
        {
            var updateCommand = new UpdateProductCommand(request.CorrelationId, request.ProductCode, request.ProductName);
            var operation = await _mediator.Send(updateCommand, cancellationToken);

            return operation;
        }

        private async Task<Result<Product>> InsertProductAsync(UpsertProductRequest request, CancellationToken cancellationToken)
        {
            var insertCommand = new InsertProductCommand(request.ProductCode, request.ProductName);
            var operation = await _mediator.Send(insertCommand, cancellationToken);

            return operation;
        }
    }
}