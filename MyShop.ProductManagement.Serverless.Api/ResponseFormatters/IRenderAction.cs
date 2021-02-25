using Microsoft.AspNetCore.Mvc;

namespace MyShop.ProductManagement.Serverless.Api.ResponseFormatters
{
    public interface IRenderAction<TRequest, in TResponse>
    {
        IActionResult Render(TRequest request, TResponse response);
    }
}