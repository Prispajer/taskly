using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.CreateTodo
{
    // Command for creating a new todo item
    public sealed record CreateTodoCommand(
        string Title,
        string Description,
        string Expiry
    ) : ICommand<Guid>;
}
