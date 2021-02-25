using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Handlers
{
    internal class UpsertProductHandler : IRequestHandler<UpsertProductRequest, Result<Product>>
    {
        private readonly IMediator _mediator;

        public UpsertProductHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> Handle(UpsertProductRequest request, CancellationToken cancellationToken)
        {
            var command = new UpsertProductCommand(request.ProductCode, request.ProductName);
            var operation = await _mediator.Send(command, cancellationToken);

            return operation;
        }
    }
}