using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.Handlers
{
    public class UpdateProductDtoHandler : IRequestHandler<UpdateProductDto, Result<GetProductResponse>>
    {
        private readonly IMediator _mediator;

        public UpdateProductDtoHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<GetProductResponse>> Handle(UpdateProductDto request, CancellationToken cancellationToken)
        {
            var serviceRequest = new UpdateProductRequest
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