using System.Threading.Tasks;
using MyShop.ProductManagement.Domain;

namespace MyShop.ProductManagement.Messaging.Handlers
{
    public interface IOrderedMessagePublisher
    {
        Task<Result> PublishAsync<TMessage>(TMessage message) where TMessage : OrderedMessageBase;
    }
}