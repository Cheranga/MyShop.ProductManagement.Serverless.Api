namespace MyShop.ProductManagement.Domain.Validators
{
    public interface IValidatableRequest
    {
        string CorrelationId { get; }
    }
}