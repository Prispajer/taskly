using Taskly.Application.Abstractions.Messaging;
using Taskly.Application.Todos.Common;

namespace Taskly.Application.Todos.GetTodos
{
    // Query for retrieving all todo items
    public sealed record GetTodosQuery() : IQuery<List<TodoResponse>>;
}
