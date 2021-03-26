using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.Handlers
{
    public class GetProductByCodeHandler : IRequestHandler<GetProductByCodeDto, Result<Product>>
    {
        private readonly IMediator _mediator;

        public GetProductByCodeHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> Handle(GetProductByCodeDto request, CancellationToken cancellationToken)
        {
            var serviceRequest = new GetProductByCodeRequest(request.CorrelationId, request.ProductCode);

            var operation = await _mediator.Send(serviceRequest, cancellationToken);
            return operation;
        }
    }
}