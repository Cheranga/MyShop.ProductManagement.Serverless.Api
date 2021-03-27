using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.ResponseFormatters
{
    public class DisplayProductFormatter : IRenderAction<GetProductByCodeDto, Result<Product>>
    {
        public IActionResult Render(GetProductByCodeDto request, Result<Product> response)
        {
            if (!response.Status)
            {
                var errorResponse = new ErrorResponse
                {
                    CorrelationId = request?.CorrelationId,
                    Message = "Error when getting the product.",
                    Errors = response.Validation.Errors.Select(x => x.ErrorMessage).ToList()
                };

                return new ObjectResult(errorResponse)
                {
                    StatusCode = (int) HttpStatusCode.BadRequest
                };
            }

            var product = response.Data;
            if (product == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(new
            {
                product = product
            });
        }
    }
}