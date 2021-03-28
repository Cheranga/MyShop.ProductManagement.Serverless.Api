using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Domain;
using Newtonsoft.Json;

namespace MyShop.ProductManagement.Serverless.Api.Services
{
    public class MessageReader : IMessageReader
    {
        private readonly ILogger<MessageReader> _logger;

        public MessageReader(ILogger<MessageReader> logger)
        {
            _logger = logger;
        }

        public Task<Result<T>> GetModelAsync<T>(Message message) where T : class
        {
            try
            {
                var messageContent = Encoding.UTF8.GetString(message.Body);
                var model = JsonConvert.DeserializeObject<T>(messageContent);
                if (model == null)
                {
                    return Task.FromResult(Result<T>.Failure(ErrorCodes.MessageReadError, "Invalid message content."));
                }

                return Task.FromResult(Result<T>.Success(model));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error when reading the message.");
            }

            return Task.FromResult(Result<T>.Failure(ErrorCodes.MessageReadError, "Error when reading the message."));
        }
    }
}