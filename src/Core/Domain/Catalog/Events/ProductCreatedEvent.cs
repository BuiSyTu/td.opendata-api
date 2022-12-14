using TD.OpenData.WebApi.Domain.Common.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog.Events;

public class ProductCreatedEvent : DomainEvent
{
    public ProductCreatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}