using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Serverless.Api.Services
{
    public interface IMessageReader
    {
        Task<Result<T>> GetModelAsync<T>(Message message) where T : class;
    }
}