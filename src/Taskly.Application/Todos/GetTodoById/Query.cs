using Taskly.Application.Abstractions.Messaging;
using Taskly.Application.Todos.Common;

namespace Taskly.Application.Todos.GetTodoById
{
    // Query for retrieving a todo item by ID
    public sealed record GetTodoByIdQuery(Guid Id) : IQuery<TodoResponse>;
}
