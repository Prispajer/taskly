using FluentValidation;

namespace Taskly.Application.Todos.UpdateTodo
{
    // Validator for UpdateTodoCommand
    public sealed class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
    {
        public UpdateTodoCommandValidator()
        {
            // Validate non-empty GUID
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Todo Id must be a non-empty GUID.");

            // Validate Title: required, length 3–100
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            // Validate Description: required, length 5–500
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(5).WithMessage("Description must be at least 5 characters long.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            // Validate Expiry: required, valid date, today or future
            RuleFor(c => c.Expiry)
                .NotEmpty().WithMessage("Expiry is required.")
                .Must(BeAValidDate).WithMessage("Expiry must be a valid date.")
                .Must(BeInFutureOrToday).WithMessage("Expiry must be today or in the future.");
        }

        // Checks if string is a valid date
        private bool BeAValidDate(string expiry)
            => DateTime.TryParse(expiry, out _);

        // Checks if date is today or in the future
        private bool BeInFutureOrToday(string expiry)
            => DateTime.TryParse(expiry, out var date) && date.Date >= DateTime.UtcNow.Date;
    }
}
