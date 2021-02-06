using FluentValidation.Results;

namespace MyShop.ProductManagement.Services.Responses
{
    public class GetProductResponse
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}