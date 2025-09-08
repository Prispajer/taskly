using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Todos.MarkTodoAsDone;
using Taskly.Domain.Todos;

public class MarkTodoAsDoneCommandHandlerTests
{
    private readonly TasklyDbContext _dbContext;
    private readonly MarkTodoAsDoneCommandHandler _handler;

    public MarkTodoAsDoneCommandHandlerTests()
    {
        // Setup: Create an in-memory database for isolated testing
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TasklyDbContext(options);
        _handler = new MarkTodoAsDoneCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Should_Mark_Todo_As_Done_When_It_Exists()
    {
        // Arrange: Add a todo to the database
        var todo = new Todo
        {
            Title = "Test",
            Description = "Desc",
            Expiry = DateTime.UtcNow.AddDays(1),
            PercentComplete = 0
        };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var command = new MarkTodoAsDoneCommand(todo.Id);

        // Act: Execute the handler
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Todo should be marked as 100% complete
        result.IsSuccess.Should().BeTrue();
        var updated = await _dbContext.Todos.FindAsync(todo.Id);
        updated!.PercentComplete.Should().Be(100);
    }

    [Fact]
    public async Task Should_Return_NotFound_When_Todo_Does_Not_Exist()
    {
        // Arrange: Create a command with a random GUID
        var command = new MarkTodoAsDoneCommand(Guid.NewGuid());

        // Act: Execute the handler
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Expect failure with NotFound error
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Todo.NotFound");
    }
}
