using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Taskly.Application.Abstractions.Data;
using Taskly.Application.Todos.MarkTodoAsDone;
using Taskly.Infrastructure.Persistence;
using Taskly.Infrastructure.Persistence.Data;
using Taskly.Infrastructure.Persistence.Repositories;
using Taskly.Domain.Todos.Entities;
using Taskly.Domain.Todos.ValueObjects;

public class MarkTodoAsDoneCommandHandlerTests
{
    private readonly TasklyDbContext _dbContext;
    private readonly ITodoRepository _repository;
    private readonly IQueryExecutor _queryExecutor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly MarkTodoAsDoneCommandHandler _handler;

    public MarkTodoAsDoneCommandHandlerTests()
    {
        // Setup: Create an in-memory database for isolated testing
        var options = new DbContextOptionsBuilder<TasklyDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TasklyDbContext(options);
        _repository = new TodoRepository(_dbContext);
        _queryExecutor = new QueryExecutor();
        _unitOfWork = _dbContext;
        _handler = new MarkTodoAsDoneCommandHandler(_repository, _queryExecutor, _unitOfWork);
    }

    [Fact]
    public async Task Should_Mark_Todo_As_Done_When_It_Exists()
    {
        // Arrange: Add a todo and set some progress first (MarkAsDone rejects 0%)
        var todo = Todo.Create("Test", "Desc", Expiry.Create(DateTime.UtcNow.AddDays(1)));
        
        _repository.Add(todo.Value);
        await _dbContext.SaveChangesAsync();

        // Set progress > 0% so MarkAsDone can succeed
        todo.Value.SetPercentComplete(Percent.Create(50).Value);
        await _dbContext.SaveChangesAsync();

        var command = new MarkTodoAsDoneCommand(todo.Value.Id);

        // Act: Execute the handler
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert: Todo should be marked as 100% complete
        result.IsSuccess.Should().BeTrue();
        var updated = await _dbContext.Todos.FindAsync(todo.Value.Id);
        updated!.PercentComplete.Value.Should().Be(100);
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
