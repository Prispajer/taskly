using FluentValidation.TestHelper;
using Taskly.Application.Todos.GetTodoById;

public class GetTodoByIdValidatorTests
{
    private readonly GetTodoByIdValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        // Arrange: Create a query with an empty Guid
        var query = new GetTodoByIdQuery(Guid.Empty);

        // Act: Validate the query
        var result = _validator.TestValidate(query);

        // Assert: Expect a validation error for the Id field
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        // Arrange: Create a query with a valid Guid
        var query = new GetTodoByIdQuery(Guid.NewGuid());

        // Act: Validate the query
        var result = _validator.TestValidate(query);

        // Assert: Expect no validation errors
        result.ShouldNotHaveAnyValidationErrors();
    }
}
