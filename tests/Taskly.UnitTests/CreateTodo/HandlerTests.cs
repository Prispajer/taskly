using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Todos.CreateTodo;

public class CreateTodoCommandHandlerTests
{
    private readonly CreateTodoCommandHandler _handler;
    private readonly TasklyDbContext _dbContext;

    public CreateTodoCommandHandlerTests()
    {
        // Create a unique in-memory database for each test class instance
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TasklyDbContext(options);
        _handler = new CreateTodoCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Should_Create_Todo_When_Command_Is_Valid()
    {
        // Arrange: Create a valid command
        var command = new CreateTodoCommand("Title", "Desc", DateTime.UtcNow.AddDays(1).ToString());

        // Act: Handle the command
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Todo should be created successfully
        result.IsSuccess.Should().BeTrue();
        _dbContext.Todos.Should().ContainSingle(t => t.Title == "Title");
    }

    [Fact]
    public async Task Should_Fail_When_Expiry_Is_Past()
    {
        // Arrange: Create a command with past expiry date
        var command = new CreateTodoCommand("Title", "Desc", DateTime.UtcNow.AddDays(-1).ToString());

        // Act: Handle the command
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Command should fail with appropriate error
        result.IsSuccess.Should().BeFalse();
        result.Error.Description.Should().Be("Expiry date cannot be in the past");
    }
}
