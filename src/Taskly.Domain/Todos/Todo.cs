namespace Taskly.Domain.Todos
{
    // Domain entity representing a todo item
    public class Todo
    {
        // Unique identifier
        public Guid Id { get; set; }

        // Task title
        public string Title { get; set; } = string.Empty;

        // Task description
        public string Description { get; set; } = string.Empty;

        // Expiry date and time
        public DateTime Expiry { get; set; }

        // Completion percentage (0–100)
        public int PercentComplete { get; set; }
    }
}
