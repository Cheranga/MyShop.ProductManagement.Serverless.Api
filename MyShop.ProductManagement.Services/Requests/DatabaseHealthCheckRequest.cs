using System.Collections.Generic;
using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Requests
{
    public class DatabaseHealthCheckRequest : IRequest<Result<Product>>, IValidatableRequest
    {
        public string CorrelationId { get; }
        public int Count { get; }

        public DatabaseHealthCheckRequest(string correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}