using FluentValidation.TestHelper;
using Taskly.Application.Todos.MarkTodoAsDone;

public class MarkTodoAsDoneValidatorTests
{
    private readonly MarkTodoAsDoneValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_For_Valid_Id()
    {
        // Arrange: Create a command with a valid GUID
        var command = new MarkTodoAsDoneCommand(Guid.NewGuid());

        // Act: Validate the command
        var result = _validator.TestValidate(command);

        // Assert: Expect no validation errors
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_Validation_For_Empty_Id()
    {
        // Arrange: Create a command with an empty GUID
        var command = new MarkTodoAsDoneCommand(Guid.Empty);

        // Act: Validate the command
        var result = _validator.TestValidate(command);

        // Assert: Expect a validation error on Id
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }
}
