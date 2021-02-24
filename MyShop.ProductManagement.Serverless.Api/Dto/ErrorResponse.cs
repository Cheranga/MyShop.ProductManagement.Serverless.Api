using System.Collections.Generic;

namespace MyShop.ProductManagement.Serverless.Api.Dto
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}