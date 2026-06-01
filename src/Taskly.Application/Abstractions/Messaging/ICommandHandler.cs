using Taskly.SharedKernel.Common;

namespace Taskly.Application.Abstractions.Messaging
{
    // Handler for commands without response
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
    }

    // Handler for commands with typed response
    public interface ICommandHandler<in TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
