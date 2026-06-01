using FluentValidation;

namespace Taskly.Application.Todos.GetIncomingTodos
{
    // Validator for GetIncomingTodosCommand
    public sealed class GetIncomingTodosValidator : AbstractValidator<GetIncomingTodosQuery>
    {
        public GetIncomingTodosValidator()
        {
            // Validate that Range is a valid enum value
            RuleFor(c => c.Range)
                .IsInEnum()
                .WithMessage("Range must be one of: Today, NextDay, CurrentWeek");
        }
    }
}
