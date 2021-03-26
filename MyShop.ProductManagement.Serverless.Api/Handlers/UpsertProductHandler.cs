using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.Handlers
{
    public class UpsertProductHandler : IRequestHandler<UpsertProductDto, Result<Product>>
    {
        private readonly IMediator _mediator;

        public UpsertProductHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> Handle(UpsertProductDto request, CancellationToken cancellationToken)
        {
            var serviceRequest = new UpsertProductRequest
            {
                CorrelationId = request.CorrelationId,
                ProductCode = request.ProductCode,
                ProductName = request.ProductDescription
            };

            var operation = await _mediator.Send(serviceRequest, cancellationToken);
            return operation;
        }
    }
}