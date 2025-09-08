using FluentValidation.TestHelper;
using Taskly.Application.Todos.CreateTodo;

public class CreateTodoCommandValidatorTests
{
    private readonly CreateTodoCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        // Arrange: Create command with empty title
        var command = new CreateTodoCommand("", "Valid description", DateTime.UtcNow.AddDays(1).ToString());

        // Act & Assert: Expect validation error for Title
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Expiry_Is_Past()
    {
        // Arrange: Create command with past expiry date
        var command = new CreateTodoCommand("Valid Title", "Valid description", DateTime.UtcNow.AddDays(-1).ToString());

        // Act & Assert: Expect validation error for Expiry
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Expiry);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange: Create a fully valid command
        var command = new CreateTodoCommand("Valid Title", "Valid description", DateTime.UtcNow.AddDays(1).ToString());

        // Act & Assert: Expect no validation errors
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
