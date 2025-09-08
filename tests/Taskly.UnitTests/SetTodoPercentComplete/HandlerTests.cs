using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Todos.SetTodoPercentComplete;
using Taskly.Domain.Todos;

public class SetTodoPercentCompleteCommandHandlerTests
{
    private readonly TasklyDbContext _dbContext;
    private readonly SetTodoPercentCompleteCommandHandler _handler;

    public SetTodoPercentCompleteCommandHandlerTests()
    {
        // Setup: Use in-memory database for isolated testing
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TasklyDbContext(options);
        _handler = new SetTodoPercentCompleteCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Should_Update_Percent_When_Todo_Exists()
    {
        // Arrange: Add a todo item to the database
        var todo = new Todo
        {
            Title = "Test",
            Description = "Desc",
            Expiry = DateTime.UtcNow.AddDays(1),
            PercentComplete = 0
        };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        // Act: Send command to update percent
        var command = new SetTodoPercentCompleteCommand(todo.Id, 60);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: PercentComplete should be updated
        result.IsSuccess.Should().BeTrue();
        var updated = await _dbContext.Todos.FindAsync(todo.Id);
        updated!.PercentComplete.Should().Be(60);
    }

    [Fact]
    public async Task Should_Return_NotFound_When_Todo_Does_Not_Exist()
    {
        // Arrange: Use a random GUID
        var command = new SetTodoPercentCompleteCommand(Guid.NewGuid(), 50);

        // Act: Try to update non-existent todo
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Should return failure with NotFound error
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Todo.NotFound");
    }
}
