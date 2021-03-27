using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Application.DataAccess;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Application.Handlers
{
    internal class DatabaseHealthCheckRequestHandler : IRequestHandler<DatabaseHealthCheckRequest, Result<Product>>
    {
        private readonly IMediator _mediator;

        public DatabaseHealthCheckRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> Handle(DatabaseHealthCheckRequest request, CancellationToken cancellationToken)
        {
            var query = new GetProductByCodeQuery(request.CorrelationId, "BLAHBLAH");
            var operation = await _mediator.Send(query, cancellationToken);

            return operation;
        }
    }
}