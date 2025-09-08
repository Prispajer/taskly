using FluentValidation;

namespace Taskly.Application.Todos.GetTodoById
{
    // Validator for GetTodoByIdQuery
    public sealed class GetTodoByIdValidator : AbstractValidator<GetTodoByIdQuery>
    {
        public GetTodoByIdValidator()
        {
            // Validate non-empty GUID
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Todo Id must be a non-empty GUID.");
        }
    }
}
