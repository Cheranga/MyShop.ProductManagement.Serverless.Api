using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MyShop.ProductManagement.Serverless.Api.Extensions
{
    public static class HttpExtensions
    {
        public static string GetHeaderValue(this HttpRequest request, string headerName)
        {
            if (request.Headers.TryGetValue(headerName, out var headerValueData))
            {
                return headerValueData.FirstOrDefault();
            }

            return string.Empty;
        }

        public static async Task<TModel> ToModel<TModel>(this HttpRequest request) where TModel : class, new()
        {
            try
            {
                var content = await new StreamReader(request.Body).ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new TModel();
                }

                var model = JsonConvert.DeserializeObject<TModel>(content);
                return model;
            }
            catch
            {
                return new TModel();
            }
        }
    }
}