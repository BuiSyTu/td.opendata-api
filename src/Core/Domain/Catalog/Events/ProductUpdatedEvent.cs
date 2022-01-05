using TD.OpenData.WebApi.Domain.Common.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog.Events;

public class ProductUpdatedEvent : DomainEvent
{
    public ProductUpdatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}