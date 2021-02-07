using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MyShop.ProductManagement.Application.Interfaces;
using MyShop.ProductManagement.Application.Requests;
using MyShop.ProductManagement.DataAccess.Queries;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.DataAccess.Services
{
    internal class GetProductDataService : IGetProductDataService
    {
        private readonly IMediator _mediator;

        public GetProductDataService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<Product>> GetProductAsync(GetProductByCodeRequest request, CancellationToken cancellationToken)
        {
            var query = new GetProductQuery(request.ProductCode);
            var operation = await _mediator.Send(query, cancellationToken);

            if (!operation.Status)
            {
                return Result<Product>.Failure("", "Error occured when getting the product.");
            }

            return Result<Product>.Success(operation.Data);
        }
    }
}