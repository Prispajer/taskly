using FluentValidation.TestHelper;
using Taskly.Application.Todos.SetTodoPercentComplete;

public class SetTodoPercentCompleteValidatorTests
{
    private readonly SetTodoPercentCompleteValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_For_Valid_Data()
    {
        // Arrange: Create a valid command
        var command = new SetTodoPercentCompleteCommand(Guid.NewGuid(), 75);

        // Act: Validate the command
        var result = _validator.TestValidate(command);

        // Assert: Expect no validation errors
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_Validation_For_Empty_Id()
    {
        // Arrange: Use an empty GUID
        var command = new SetTodoPercentCompleteCommand(Guid.Empty, 50);

        // Act: Validate the command
        var result = _validator.TestValidate(command);

        // Assert: Expect a validation error for Id
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Should_Fail_Validation_For_Invalid_Percent(int percent)
    {
        // Arrange: Use invalid percent values
        var command = new SetTodoPercentCompleteCommand(Guid.NewGuid(), percent);

        // Act: Validate the command
        var result = _validator.TestValidate(command);

        // Assert: Expect a validation error for PercentComplete
        result.ShouldHaveValidationErrorFor(c => c.PercentComplete);
    }
}
