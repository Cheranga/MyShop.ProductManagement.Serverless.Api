using System.Text.Json.Serialization;
using MediatR;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Requests
{
    public class UpsertProductRequest : IRequest<Result<Product>>, IValidatableRequest
    {
        public string CorrelationId { get; set; }

        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}