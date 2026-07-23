using Taskly.Application.Abstractions.Data;
using Taskly.Domain.Todos.Entities;

namespace Taskly.Infrastructure.Persistence.Repositories;

public class TodoRepository(TasklyDbContext context) : ITodoRepository
{
    public IQueryable<Todo> GetQueryable() => context.Todos;
    public void Add(Todo todo) => context.Add(todo);
    public void Remove(Todo todo) => context.Remove(todo);
}   