using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Handlers
{
    internal class DatabaseHealthCheckRequestHandler : IRequestHandler<DatabaseHealthCheckRequest, Result<GetProductResponse>>
    {
        private readonly IMediator _mediator;

        public DatabaseHealthCheckRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<GetProductResponse>> Handle(DatabaseHealthCheckRequest request, CancellationToken cancellationToken)
        {
            var query = new GetProductByCodeQuery(request.CorrelationId, "BLAHBLAH");
            var operation = await _mediator.Send(query, cancellationToken);

            if (!operation.Status)
            {
                return Result<GetProductResponse>.Failure(operation.ErrorCode, operation.Validation);
            }

            var getProductResponse = new GetProductResponse
            {
                ProductCode = "HEALTH_CHECK",
                ProductName = "HEATH_CHECK"
            };

            return Result<GetProductResponse>.Success(getProductResponse);
        }
    }
}