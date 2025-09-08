using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Todos.DeleteTodo;
using Taskly.Domain.Todos;

public class DeleteTodoCommandHandlerTests
{
    private readonly TasklyDbContext _dbContext;
    private readonly DeleteTodoCommandHandler _handler;

    public DeleteTodoCommandHandlerTests()
    {
        // Create a unique in-memory database for each test class instance
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TasklyDbContext(options);
        _handler = new DeleteTodoCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Should_Delete_Todo_When_Exists()
    {
        // Arrange: Add a todo to the database
        var todo = new Todo { Title = "Test", Description = "Desc", Expiry = DateTime.UtcNow.AddDays(1) };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        // Act: Send delete command
        var command = new DeleteTodoCommand(todo.Id);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Todo should be deleted successfully
        result.IsSuccess.Should().BeTrue();
        _dbContext.Todos.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Return_NotFound_When_Todo_Does_Not_Exist()
    {
        // Arrange: Create a command with a random ID
        var command = new DeleteTodoCommand(Guid.NewGuid());

        // Act: Handle the command
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Should return a not found error
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Todo.NotFound");
    }
}
