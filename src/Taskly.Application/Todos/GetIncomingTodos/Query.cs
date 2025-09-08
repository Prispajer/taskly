using Tasky.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.GetIncomingTodo
{
    // Command for retrieving incoming todos based on time range
    public sealed record GetIncomingTodosQuery(IncomingTodoRange Range)
        : IQuery<List<TodoResponse>>;
}
