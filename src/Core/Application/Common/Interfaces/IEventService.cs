using TD.OpenData.WebApi.Domain.Common.Contracts;

namespace TD.OpenData.WebApi.Application.Common.Interfaces;

public interface IEventService : ITransientService
{
    Task PublishAsync(DomainEvent domainEvent);
}