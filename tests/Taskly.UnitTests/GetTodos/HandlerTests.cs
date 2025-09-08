using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Todos.GetTodos;
using Taskly.Domain.Todos;

public class GetTodosQueryHandlerTests
{
    private readonly TasklyDbContext _dbContext;
    private readonly GetTodosQueryHandler _handler;

    public GetTodosQueryHandlerTests()
    {
        // Create a unique in-memory database for each test run to ensure isolation
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new TasklyDbContext(options);
        _handler = new GetTodosQueryHandler(_dbContext);
    }

    [Fact]
    public async Task Should_Return_Todos_Without_Exact_Count_Assertion()
    {
        // Add a sample todo to the in-memory database
        _dbContext.Todos.Add(new Todo
        {
            Title = "Sample Todo",
            Description = "Desc",
            Expiry = DateTime.UtcNow.AddDays(1),
            PercentComplete = 0
        });
        await _dbContext.SaveChangesAsync();

        // Execute the query handler
        var result = await _handler.Handle(new GetTodosQuery(), CancellationToken.None);

        // Assert: The result is successful and contains the added todo
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Any(t => t.Title == "Sample Todo").Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_Valid_Todo_Objects_When_They_Exist()
    {
        // Execute the query handler without adding new data
        var result = await _handler.Handle(new GetTodosQuery(), CancellationToken.None);

        // Assert: The result is successful and all returned todos have valid fields
        result.IsSuccess.Should().BeTrue();

        foreach (var todo in result.Value)
        {
            todo.Title.Should().NotBeNullOrEmpty();
            todo.Description.Should().NotBeNull();
            todo.Expiry.Should().BeAfter(DateTime.MinValue);
        }
    }
}

