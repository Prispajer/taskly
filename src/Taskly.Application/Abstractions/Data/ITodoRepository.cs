using Taskly.Application.Todos.Common;
using Taskly.Domain.Todos.Entities;

namespace Taskly.Application.Abstractions.Data;

public interface ITodoRepository
{
     IQueryable<Todo> GetQueryable();
     void Add(Todo todo);
     void Remove(Todo todo);
}