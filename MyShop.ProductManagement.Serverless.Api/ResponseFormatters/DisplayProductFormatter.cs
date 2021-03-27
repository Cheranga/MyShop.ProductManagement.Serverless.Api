using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Domain.Constants;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.ResponseFormatters
{
    public class DisplayProductFormatter : IRenderAction<GetProductByCodeDto, Result<Product>>
    {
        public IActionResult Render(GetProductByCodeDto request, Result<Product> response)
        {
            if (!response.Status)
            {
                return GetErrorResponse(response);
            }

            return new OkObjectResult(response.Data);
        }

        private IActionResult GetErrorResponse(Result<Product> response)
        {
            HttpStatusCode statusCode;
            var errorCode = response.ErrorCode;

            switch (errorCode)
            {
                case Application.Constants.ErrorCodes.ProductNotFound:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case Application.Constants.ErrorCodes.DataAccessError:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;

                default:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
            }

            var errorResponse = new ErrorResponse
            {
                ErrorCode = response.ErrorCode,
                Errors = response.Validation.Errors.Select(x => new ErrorMessage
                {
                    Field = x.PropertyName,
                    Message = x.ErrorMessage
                }).ToList()
            };

            return new ObjectResult(errorResponse)
            {
                StatusCode = (int)(statusCode)
            };
        }
    }
}