using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Requests
{
    public class GetProductByCodeRequest : IRequest<Result<Product>>, IValidatableRequest
    {
        public string CorrelationId { get; }
        public string ProductCode { get; }

        public GetProductByCodeRequest(string correlationId, string productCode)
        {
            CorrelationId = correlationId;
            ProductCode = productCode;
        }
    }
}