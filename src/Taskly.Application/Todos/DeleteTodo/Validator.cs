using FluentValidation;

namespace Taskly.Application.Todos.DeleteTodo
{
    // Validator for DeleteTodoCommand
    public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
    {
        public DeleteTodoCommandValidator()
        {
            // Validate non-empty GUID
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Todo Id must be a non-empty GUID.");
        }
    }
}
