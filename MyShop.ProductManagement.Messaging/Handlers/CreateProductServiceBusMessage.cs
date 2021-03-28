namespace MyShop.ProductManagement.Messaging.Handlers
{
    public class CreateProductServiceBusMessage : OrderedMessageBase
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public override string MessageType { get; set; } = "CreateProduct";

        public override string GetSessionId()
        {
            return ProductCode;
        }
    }
}