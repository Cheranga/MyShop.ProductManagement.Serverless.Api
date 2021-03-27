using MediatR;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Requests
{
    public class DatabaseHealthCheckRequest : IRequest<Result<GetProductResponse>>, IValidatableRequest
    {
        public DatabaseHealthCheckRequest(string correlationId)
        {
            CorrelationId = correlationId;
        }

        public string CorrelationId { get; }
    }
}