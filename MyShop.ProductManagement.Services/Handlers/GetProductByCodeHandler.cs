using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Handlers
{
    internal class GetProductByCodeHandler : IRequestHandler<GetProductByCodeRequest, Result<Product>>
    {
        private readonly IMediator _mediator;

        public GetProductByCodeHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> Handle(GetProductByCodeRequest request, CancellationToken cancellationToken)
        {
            var query = new GetProductQuery(request?.ProductCode);
            var operation = await _mediator.Send(query, cancellationToken);

            return operation;
        }
    }
}