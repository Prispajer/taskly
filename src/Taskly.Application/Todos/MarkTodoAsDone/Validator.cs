using FluentValidation;

namespace Taskly.Application.Todos.MarkTodoAsDone
{
    // Validator for MarkTodoAsDoneCommand
    public sealed class MarkTodoAsDoneValidator : AbstractValidator<MarkTodoAsDoneCommand>
    {
        public MarkTodoAsDoneValidator()
        {
            // Validate non-empty GUID
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Todo Id must be a non-empty GUID.");
        }
    }
}
