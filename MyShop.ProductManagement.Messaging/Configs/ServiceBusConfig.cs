namespace MyShop.ProductManagement.Messaging.Configs
{
    public class ServiceBusConfig
    {
        public string WriteTopic { get; set; }
        public string ListeningSubscription { get; set; }
    }
}