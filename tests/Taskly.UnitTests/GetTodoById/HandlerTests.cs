using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Todos.GetTodoById;
using Taskly.Domain.Todos;

public class GetTodoByIdHandlerTests
{
    private readonly TasklyDbContext _dbContext;
    private readonly GetTodoByIdQueryHandler _handler;

    public GetTodoByIdHandlerTests()
    {
        // Create a unique in-memory database for each test class instance
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TasklyDbContext(options);
        _handler = new GetTodoByIdQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Should_Return_Todo_When_Exists()
    {
        // Arrange: Add a todo item to the database
        var todo = new Todo
        {
            Title = "Existing Todo",
            Description = "Test",
            Expiry = DateTime.UtcNow.AddDays(1)
        };

        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var query = new GetTodoByIdQuery(todo.Id);

        // Act: Execute the query handler
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert: The result should be successful and return the correct todo
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Existing Todo");
    }

    [Fact]
    public async Task Should_Return_NotFound_When_Todo_Does_Not_Exist()
    {
        // Arrange: Create a query with a random ID
        var query = new GetTodoByIdQuery(Guid.NewGuid());

        // Act: Execute the query handler
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert: The result should be a failure with a "Todo.NotFound" error code
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Todo.NotFound");
    }
}
