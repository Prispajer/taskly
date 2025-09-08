using FluentValidation;

namespace Taskly.Application.Todos.SetTodoPercentComplete
{
    // Validator for SetTodoPercentCompleteCommand
    public class SetTodoPercentCompleteValidator : AbstractValidator<SetTodoPercentCompleteCommand>
    {
        public SetTodoPercentCompleteValidator()
        {
            // Validate non-empty GUID
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Todo Id must be a non-empty GUID.");

            // Validate PercentComplete: required, range 0–100
            RuleFor(c => c.PercentComplete)
                .NotNull().WithMessage("PercentComplete is required.")
                .InclusiveBetween(0, 100).WithMessage("PercentComplete must be between 0 and 100.");
        }
    }
}
