using FluentValidation.TestHelper;
using Taskly.Application.Todos.UpdateTodo;

public class UpdateTodoCommandValidatorTests
{
    private readonly UpdateTodoCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_When_All_Fields_Are_Valid()
    {
        // Arrange: Create a valid command with proper values
        var command = new UpdateTodoCommand(
            Guid.NewGuid(),
            "Valid title with enough length",
            "Valid description with enough detail",
            DateTime.UtcNow.AddDays(1).ToString("o")
        );

        // Act: Validate the command
        var result = _validator.TestValidate(command);

        // Assert: Expect no validation errors
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_Validation_When_Id_Is_Empty()
    {
        // Arrange: Use an empty GUID
        var command = new UpdateTodoCommand(
            Guid.Empty,
            "Valid title",
            "Valid description",
            DateTime.UtcNow.AddDays(1).ToString("o")
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Hi")]
    public void Should_Fail_Validation_When_Title_Is_Too_Short(string title)
    {
        // Arrange: Provide invalid title
        var command = new UpdateTodoCommand(
            Guid.NewGuid(),
            title,
            "Valid description",
            DateTime.UtcNow.AddDays(1).ToString("o")
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Bad")]
    public void Should_Fail_Validation_When_Description_Is_Too_Short(string description)
    {
        // Arrange: Provide invalid description
        var command = new UpdateTodoCommand(
            Guid.NewGuid(),
            "Valid title",
            description,
            DateTime.UtcNow.AddDays(1).ToString("o")
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Description);
    }

    [Theory]
    [InlineData("not-a-date")]
    [InlineData("2000-01-01T00:00:00Z")]
    public void Should_Fail_Validation_When_Expiry_Is_Invalid(string expiry)
    {
        // Arrange: Provide invalid expiry date
        var command = new UpdateTodoCommand(
            Guid.NewGuid(),
            "Valid title",
            "Valid description",
            expiry
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Expiry);
    }
}
