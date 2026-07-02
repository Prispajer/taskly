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
        // Find all aggregate roots with domain events
        var aggregateRoots = context.ChangeTracker.Entries<AggregateRoot<Guid>>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        if (!aggregateRoots.Any()) return;

        // Collect all domain events
        var domainEvents = aggregateRoots
            .SelectMany(ar => ar.DomainEvents)
            .ToList();

        // Publish each domain event
        foreach (var domainEvent in domainEvents)
        {
            try
            {
                await _domainEventPublisher.PublishAsync(domainEvent, cancellationToken);
                _logger.LogInformation("Successfully published domain event {EventType} with ID {EventId}",
                    domainEvent.GetType().Name, domainEvent.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish domain event {EventType} with ID {EventId}",
                    domainEvent.GetType().Name, domainEvent.Id);

                // Re-add events to aggregate if publishing fails
                // This ensures events aren't lost if there's a publishing failure
                foreach (var aggregateRoot in aggregateRoots)
                {
                    aggregateRoot.AddDomainEvent(domainEvent);
                }

                throw; // Re-throw to prevent saving if event publishing fails
            }
        }
        
        // Clear events from aggregates before publishing to prevent duplicate publishing
        foreach (var aggregateRoot in aggregateRoots)
        {
            aggregateRoot.ClearDomainEvents();
        }
    }
}