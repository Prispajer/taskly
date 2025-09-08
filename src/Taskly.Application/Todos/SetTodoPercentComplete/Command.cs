using Tasky.Application.Abstractions.Messaging;

namespace Taskly.Application.Todos.SetTodoPercentComplete
{
    // Command for setting percent complete on a todo item
    public sealed record SetTodoPercentCompleteCommand(
        Guid Id,
        int PercentComplete
    ) : ICommand;
}
