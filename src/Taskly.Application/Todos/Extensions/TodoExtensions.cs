using Taskly.Application.Todos.Common;
using Taskly.Domain.Todos.Entities;

namespace Taskly.Application.Todos.Extensions;

public static class TodoExtensions
{
    public static IQueryable<TodoResponse> ToResponse(this IQueryable<Todo> query)
    {
        return query.Select(t => new TodoResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Expiry = t.Expiry.Value,
            PercentComplete = t.PercentComplete.Value
        });
    }
}