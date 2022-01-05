using TD.OpenData.WebApi.Domain.Common.Contracts;
using MediatR;

namespace TD.OpenData.WebApi.Application.Common.Events;

public class EventNotification<T> : INotification
where T : DomainEvent
{
    public EventNotification(T domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public T DomainEvent { get; }
}