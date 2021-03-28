using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Messaging.Configs;
using Newtonsoft.Json;

namespace MyShop.ProductManagement.Messaging.Handlers
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly ITopicClient _topicClient;
        private readonly ILogger<MessagePublisher> _logger;

        public MessagePublisher(ITopicClient topicClient, ILogger<MessagePublisher> logger)
        {
            _topicClient = topicClient;
            _logger = logger;
        }

        public async Task<Result> PublishAsync<TMessage>(TMessage message) where TMessage : MessageBase
        {   
            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                var serviceBusMessage = new Message(messageBytes)
                {
                    Label = message.GetType().Name,
                    ContentType = "application/json",
                    CorrelationId = message.CorrelationId
                };

                await _topicClient.SendAsync(serviceBusMessage);
                await _topicClient.CloseAsync();

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when publishing message.");
            }
            finally
            {
                await _topicClient.CloseAsync();
            }

            return Result.Failure(ErrorCodes.MessagePublishError, "Error occured when publishing message.");
        }
    }
}