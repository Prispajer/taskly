using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.UpdateTodo
{
    // Command for updating a todo item
    public sealed record UpdateTodoCommand(
        Guid Id,
        string Title,
        string Description,
        string Expiry
    ) : ICommand;
}
