using FluentValidation.TestHelper;
using Taskly.Application.Todos.DeleteTodo;

public class DeleteTodoCommandValidatorTests
{
    private readonly DeleteTodoCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        // Arrange: Create command with empty Guid
        var command = new DeleteTodoCommand(Guid.Empty);

        // Act & Assert: Expect validation error for Id
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        // Arrange: Create command with valid Guid
        var command = new DeleteTodoCommand(Guid.NewGuid());

        // Act & Assert: Expect no validation errors
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
