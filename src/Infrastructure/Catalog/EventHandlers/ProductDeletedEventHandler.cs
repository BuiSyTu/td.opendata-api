using TD.OpenData.WebApi.Application.Common.Events;
using TD.OpenData.WebApi.Domain.Catalog.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.EventHandlers;

public class ProductDeletedEventHandler : INotificationHandler<EventNotification<ProductDeletedEvent>>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger;

    public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EventNotification<ProductDeletedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", notification.DomainEvent.GetType().Name);
        return Task.CompletedTask;
    }
}