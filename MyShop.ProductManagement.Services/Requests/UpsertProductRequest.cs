using System.Text.Json.Serialization;

namespace MyShop.ProductManagement.Application.Requests
{
    public class UpsertProductRequest
    {
        [JsonIgnore]
        public string CorrelationId { get; set; }

        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}