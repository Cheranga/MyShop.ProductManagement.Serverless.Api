using System;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using MyShop.ProductManagement.Application.Constants;
using MyShop.ProductManagement.Domain;
using MyShop.ProductManagement.Messaging.Configs;

namespace MyShop.ProductManagement.Messaging.Handlers
{
    public class OrderedMessagePublisher : IOrderedMessagePublisher
    {
        private readonly ServiceBusConfig _serviceBusConfig;
        private readonly ServiceBusClient _serviceBusClient;

        private readonly ILogger<OrderedMessagePublisher> _logger;

        public OrderedMessagePublisher(ServiceBusConfig serviceBusConfig, ServiceBusClient serviceBusClient, ILogger<OrderedMessagePublisher> logger)
        {
            _serviceBusConfig = serviceBusConfig;
            _serviceBusClient = serviceBusClient;
            _logger = logger;
        }

        public async Task<Result> PublishAsync<TMessage>(TMessage message) where TMessage : OrderedMessageBase
        {
            var sender = _serviceBusClient.CreateSender(_serviceBusConfig.WriteTopic);

            try
            {
                var sbMessage = new ServiceBusMessage(JsonSerializer.SerializeToUtf8Bytes(message));
                sbMessage.ApplicationProperties.Add("MessageType", message.MessageType);
                sbMessage.Subject = message.GetType().Name;
                sbMessage.SessionId = message.GetSessionId();
                sbMessage.ContentType = MediaTypeNames.Application.Json;
                sbMessage.CorrelationId = message.CorrelationId;
                
                await sender.SendMessageAsync(sbMessage);
                
                
                
                // var serviceBusMessage = new Message(messageBytes)
                // {
                //     Label = message.GetType().Name,
                //     SessionId = message.GetSessionId(),
                //     ContentType = "application/json",
                //     CorrelationId = message.CorrelationId
                // };
                //
                // serviceBusMessage.UserProperties.Add("MessageType", message.MessageType);
                //
                // await _topicClient.SendAsync(serviceBusMessage);
                // await _topicClient.CloseAsync();

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occured when publishing message.");
            }
            finally
            {
                await sender.CloseAsync();
                // await _topicClient.CloseAsync();
            }

            return Result.Failure(ErrorCodes.MessagePublishError, "Error occured when publishing message.");
        }
    }
}