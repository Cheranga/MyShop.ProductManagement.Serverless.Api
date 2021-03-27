using System.Collections.Generic;

namespace MyShop.ProductManagement.Serverless.Api.Dto
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public List<ErrorMessage> Errors { get; set; } = new List<ErrorMessage>();
    }
}