namespace MyShop.ProductManagement.Messaging.Handlers
{
    public class UpdateProductServiceBusMessage : OrderedMessageBase
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public override string GetSessionId()
        {
            return ProductCode;
        }

        public override string MessageType { get; set; } = "UpdateProduct";
    }
}