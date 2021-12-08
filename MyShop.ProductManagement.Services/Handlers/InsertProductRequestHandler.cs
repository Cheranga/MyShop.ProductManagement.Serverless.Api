using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Messages;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Handlers
{   
    public class InsertProductRequestHandler : IRequestHandler<InsertProductRequest, Result<GetProductResponse>>
    {
        private readonly IMediator _mediator;

        public InsertProductRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<Result<GetProductResponse>> Handle(InsertProductRequest request, CancellationToken cancellationToken)
        {
            var createProductMessage = new CreateProductMessage
            {
                CorrelationId = request.CorrelationId,
                ProductCode = request.ProductCode,
                ProductName = request.ProductName
            };

            var operation = await _mediator.Send(createProductMessage, cancellationToken);
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
    }
}