using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Todos.UpdateTodo;
using Taskly.Domain.Todos;

public class UpdateTodoCommandHandlerTests
{
    private readonly TasklyDbContext _dbContext;
    private readonly UpdateTodoCommandHandler _handler;

    public UpdateTodoCommandHandlerTests()
    {
        // Setup: Use in-memory database for isolated testing
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TasklyDbContext(options);
        _handler = new UpdateTodoCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Should_Update_Todo_When_It_Exists()
    {
        // Arrange: Add a todo item to the database
        var todo = new Todo
        {
            Title = "Old title",
            Description = "Old description",
            Expiry = DateTime.UtcNow.AddDays(1),
            PercentComplete = 0
        };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        // Act: Send update command
        var command = new UpdateTodoCommand(
            todo.Id,
            "New title",
            "New description",
            DateTime.UtcNow.AddDays(2).ToString("o")
        );
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Todo should be updated successfully
        result.IsSuccess.Should().BeTrue();
        var updated = await _dbContext.Todos.FindAsync(todo.Id);
        updated!.Title.Should().Be("New title");
        updated.Description.Should().Be("New description");
        updated.Expiry.Should().Be(DateTime.Parse(command.Expiry));
    }

    [Fact]
    public async Task Should_Return_NotFound_When_Todo_Does_Not_Exist()
    {
        // Arrange: Use a non-existent ID
        var command = new UpdateTodoCommand(
            Guid.NewGuid(),
            "Title",
            "Description",
            DateTime.UtcNow.AddDays(1).ToString("o")
        );

        // Act: Try to update a missing todo
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Should return failure with NotFound error
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Todo.NotFound");
    }
}
