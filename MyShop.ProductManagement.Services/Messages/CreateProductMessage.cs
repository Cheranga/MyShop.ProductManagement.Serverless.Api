using MediatR;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Validators;

namespace MyShop.ProductManagement.Application.Messages
{
    public class CreateProductMessage : IRequest<Result>, IValidatableRequest
    {
        public string CorrelationId { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}