using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Todos.GetIncomingTodo;
using Taskly.Domain.Todos;

// Integration tests for GetIncomingTodoHandler
public class GetIncomingTodosHandlerTests
{
    private readonly TasklyDbContext _dbContext;
    private readonly GetIncomingTodoHandler _handler;

    public GetIncomingTodosHandlerTests()
    {
        // Create a unique in-memory database for each test class instance
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TasklyDbContext(options);
        _handler = new GetIncomingTodoHandler(_dbContext);
    }

    [Fact]
    public async Task Should_Return_Todos_For_Today()
    {
        // Arrange: add a todo with today's expiry
        var todo = new Todo
        {
            Title = "Today Task",
            Description = "Due today",
            Expiry = DateTime.UtcNow.Date.AddHours(12)
        };

        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var command = new GetIncomingTodosQuery(IncomingTodoRange.Today);

        // Act: execute the query handler
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: expect one matching todo
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainSingle(t => t.Title == "Today Task");
    }

    [Fact]
    public async Task Should_Return_Empty_When_No_Todos_In_Range()
    {
        // Arrange: no todos added for the specified range
        var command = new GetIncomingTodosQuery(IncomingTodoRange.NextDay);

        // Act: execute the query handler
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: expect empty result
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
