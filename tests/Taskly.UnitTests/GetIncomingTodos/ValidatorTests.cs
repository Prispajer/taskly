using FluentValidation.TestHelper;
using Taskly.Application.Todos.GetIncomingTodo;

// Unit tests for GetIncomingTodosValidator
public class GetIncomingTodosValidatorTests
{
    private readonly GetIncomingTodosValidator _validator = new();

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Enum()
    {
        // Arrange: use a valid enum value
        var command = new GetIncomingTodosQuery(IncomingTodoRange.Today);

        // Act: validate the command
        var result = _validator.TestValidate(command);

        // Assert: no validation errors expected
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_For_Invalid_Enum_Value()
    {
        // Arrange: simulate an invalid enum value by casting out-of-range int
        var invalidRange = (IncomingTodoRange)999;
        var command = new GetIncomingTodosQuery(invalidRange);

        // Act: validate the command
        var result = _validator.TestValidate(command);

        // Assert: expect validation error for Range property
        result.ShouldHaveValidationErrorFor(c => c.Range);
    }
}
