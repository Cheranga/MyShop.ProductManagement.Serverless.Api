using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Requests
{
    public class GetProductByCodeRequest : IRequest<Result<Product>>, IValidatableRequest
    {
        [JsonIgnore]
        public string CorrelationId { get; set; }

        public string ProductCode { get; set; }
    }
}