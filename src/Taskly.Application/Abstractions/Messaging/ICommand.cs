namespace Tasky.Application.Abstractions.Messaging
{
    // Marker interface for commands without response
    public interface ICommand { }

    // Marker interface for commands with typed response
    public interface ICommand<TResponse> { }
}
