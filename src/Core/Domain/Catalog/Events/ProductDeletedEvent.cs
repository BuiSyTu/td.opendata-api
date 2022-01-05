using TD.OpenData.WebApi.Domain.Common.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog.Events;

public class ProductDeletedEvent : DomainEvent
{
    public ProductDeletedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}