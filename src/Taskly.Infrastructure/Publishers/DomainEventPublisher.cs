using MediatR;
using Taskly.SharedKernel.Interfaces;

namespace Taskly.Infrastructure.Publishers;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IPublisher _publisher;

    public DomainEventPublisher(IPublisher publisher)
    {
        _publisher = publisher;
    }
    
    public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        if (domainEvent is INotification notification)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }    
    }
}