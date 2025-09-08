using Taskly.SharedKernel;

namespace Tasky.Application.Abstractions.Messaging
{
    // Handler for queries with typed response
    public interface IQueryHandler<in TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
