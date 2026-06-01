using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.DeleteTodo
{
    // Command for deleting a todo item
    public sealed record DeleteTodoCommand(Guid Id) : ICommand;
}
