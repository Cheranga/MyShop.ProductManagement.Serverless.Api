using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Domain;
using Newtonsoft.Json;

namespace MyShop.ProductManagement.Messaging.Handlers
{
    public class OrderedMessagePublisher : IOrderedMessagePublisher
    {
        private readonly ILogger<OrderedMessagePublisher> _logger;
        private readonly ITopicClient _topicClient;

        public OrderedMessagePublisher(ITopicClient topicClient, ILogger<OrderedMessagePublisher> logger)
        {
            _topicClient = topicClient;
            _logger = logger;
        }

        public async Task<Result> PublishAsync<TMessage>(TMessage message) where TMessage : OrderedMessageBase
        {
            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                var serviceBusMessage = new Message(messageBytes)
                {
                    Label = message.GetType().Name,
                    SessionId = message.GetSessionId(),
                    ContentType = "application/json",
                    CorrelationId = message.CorrelationId
                };

                serviceBusMessage.UserProperties.Add("MessageType", message.MessageType);

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