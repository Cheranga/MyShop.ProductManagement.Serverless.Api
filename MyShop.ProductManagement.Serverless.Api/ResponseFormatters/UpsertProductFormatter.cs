using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.ResponseFormatters
{
    public class UpsertProductFormatter : IRenderAction<UpsertProductDto, Result<Product>>
    {
        public IActionResult Render(UpsertProductDto request, Result<Product> response)
        {
            if (response.Status)
            {
                return new OkObjectResult(response.Data);
            }

            var errorResponse = new ErrorResponse
            {
                CorrelationId = request?.CorrelationId,
                Message = "Error occured when upserting product.",
                Errors = response.Validation.Errors.Select(x => x.ErrorMessage).ToList()
            };

            return new ObjectResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}