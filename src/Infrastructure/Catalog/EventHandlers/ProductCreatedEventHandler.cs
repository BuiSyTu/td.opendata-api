using TD.OpenData.WebApi.Application.Common.Events;
using TD.OpenData.WebApi.Domain.Catalog.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.EventHandlers;

public class ProductCreatedEventHandler : INotificationHandler<EventNotification<ProductCreatedEvent>>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(EventNotification<ProductCreatedEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{event} Triggered", notification.DomainEvent.GetType().Name);
        return Task.CompletedTask;
    }
}