using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.GetTodos
{
    // Query for retrieving all todo items
    public sealed record GetTodosQuery() : IQuery<List<TodoResponse>>;
}
