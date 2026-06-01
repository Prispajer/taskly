using Taskly.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.MarkTodoAsDone
{
    // Command for marking a todo item as done
    public sealed record MarkTodoAsDoneCommand(Guid Id) : ICommand;
}
