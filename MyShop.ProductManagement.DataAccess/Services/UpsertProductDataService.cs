using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.DataAccess.CommandHandlers;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.DataAccess.Services
{
    internal class UpsertProductDataService : IUpsertProductDataService
    {
        private readonly IMediator _mediator;

        public UpsertProductDataService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> UpsertProductAsync(UpsertProductRequest request, CancellationToken cancellationToken)
        {
            var command = new UpsertProductCommand(request.ProductCode, request.ProductName);

            var operation = await _mediator.Send(command, cancellationToken);

            if (!operation.Status)
            {
                return Result<Product>.Failure("", "Error occured when upserting the product.");
            }

            return Result<Product>.Success(operation.Data);
        }
    }
}