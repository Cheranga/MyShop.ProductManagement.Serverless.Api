﻿namespace MyShop.ProductManagement.Messaging.Handlers
{
    public abstract class OrderedMessageBase
    {
        public string CorrelationId { get; set; }


        public abstract string MessageType { get; set; }

        public abstract string GetSessionId();
    }
}