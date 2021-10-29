using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Application.Responses;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Serverless.Api.Dto;

namespace MyShop.ProductManagement.Serverless.Api.ResponseFormatters
{
    public class InsertProductFormatter : IRenderAction<InsertProductDto, Result<GetProductResponse>>
    {
        public IActionResult Render(InsertProductDto request, Result<GetProductResponse> response)
        {
            if (!response.Status)
            {
                return GetErrorResponse(response);
            }

            return new ObjectResult(response.Data)
            {
                StatusCode = (int)(HttpStatusCode.Accepted)
            };
        }

        private IActionResult GetErrorResponse(Result<GetProductResponse> response)
        {
            HttpStatusCode statusCode;
            var errorCode = response.ErrorCode;

            switch (errorCode)
            {
                case ErrorCodes.DataAccessError:
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
                StatusCode = (int) statusCode
            };
        }
    }
}