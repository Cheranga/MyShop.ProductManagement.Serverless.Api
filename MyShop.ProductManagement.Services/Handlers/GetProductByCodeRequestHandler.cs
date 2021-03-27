using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Handlers
{
    internal class GetProductByCodeRequestHandler : IRequestHandler<GetProductByCodeRequest, Result<Product>>
    {
        private readonly IMediator _mediator;

        public GetProductByCodeRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> Handle(GetProductByCodeRequest request, CancellationToken cancellationToken)
        {
            var query = new GetProductByCodeQuery(request.CorrelationId, request.ProductCode);
            var operation = await _mediator.Send(query, cancellationToken);

            if (!operation.Status)
            {
                return operation;
            }

            var product = operation.Data;
            if (product == null)
            {
                return Result<Product>.Failure(ErrorCodes.ProductNotFound, "Product not found.");
            }

            return operation;
        }
    }
}