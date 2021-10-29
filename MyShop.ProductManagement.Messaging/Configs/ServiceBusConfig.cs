namespace MyShop.ProductManagement.Messaging.Configs
{
    public class ServiceBusConfig
    {
        public string WriteConnectionString { get; set; }
        public string ReadConnectionString { get; set; }
        public string WriteTopic { get; set; }
        public string ListeningSubscription { get; set; }
    }
}