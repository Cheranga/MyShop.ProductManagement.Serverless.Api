using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Requests
{
    public class DatabaseHealthCheckRequest : IRequest<Result<Product>>, IValidatableRequest
    {
        public DatabaseHealthCheckRequest(string correlationId)
        {
            CorrelationId = correlationId;
        }

        public int Count { get; }
        public string CorrelationId { get; }
    }
}