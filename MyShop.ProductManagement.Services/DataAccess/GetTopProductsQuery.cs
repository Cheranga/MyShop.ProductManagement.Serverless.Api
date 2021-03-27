using System.Collections.Generic;
using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.DataAccess
{
    public class GetTopProductsQuery : IRequest<Result<List<Product>>>, IValidatableRequest
    {
        public GetTopProductsQuery(string correlationId, int count)
        {
            CorrelationId = correlationId;
            Count = count;
        }

        public int Count { get; }
        public string CorrelationId { get; }
    }
}