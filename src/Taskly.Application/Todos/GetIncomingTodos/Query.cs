using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.GetIncomingTodos
{
    // Command for retrieving incoming todos based on time range
    public sealed record GetIncomingTodosQuery(IncomingTodoRange Range)
        : IQuery<List<TodoResponse>>;
}
