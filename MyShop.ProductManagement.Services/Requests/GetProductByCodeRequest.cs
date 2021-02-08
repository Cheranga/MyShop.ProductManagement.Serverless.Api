using System.Text.Json.Serialization;

namespace MyShop.ProductManagement.Application.Requests
{
    public class GetProductByCodeRequest
    {
        [JsonIgnore]
        public string CorrelationId { get; set; }

        public string ProductCode { get; set; }
    }
}