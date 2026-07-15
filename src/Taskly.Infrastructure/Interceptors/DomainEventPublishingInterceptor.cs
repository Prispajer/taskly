using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Taskly.SharedKernel.Interfaces;
using Taskly.SharedKernel.Common;

namespace Taskly.Infrastructure.Interceptors;

public class DomainEventPublishingInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ILogger<IDomainEventPublisher> _logger;

    public DomainEventPublishingInterceptor(IDomainEventPublisher domainEventPublisher,
        ILogger<IDomainEventPublisher> logger)
    {
        _domainEventPublisher = domainEventPublisher;
        _logger = logger;
    }
    
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await PublishDomainEventsAsync(eventData.Context, cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
    {
        var aggregateRoots = context.ChangeTracker.Entries<AggregateRoot<Guid>>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        if (!aggregateRoots.Any()) return;
        
        var domainEvents = aggregateRoots
            .SelectMany(ar => ar.DomainEvents)
            .ToList();
        
        _logger.LogInformation("Found {Count} events to send!", domainEvents.Count);

        foreach (var aggregateRoot in aggregateRoots)
        {
            aggregateRoot.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            await _domainEventPublisher.PublishAsync(domainEvent, cancellationToken);
            _logger.LogInformation("Successfully published {EventType}", domainEvent.GetType().Name);        
        }
    }
}