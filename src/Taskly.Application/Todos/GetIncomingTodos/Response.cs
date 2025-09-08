namespace Taskly.Application.Todos.GetIncomingTodo
{
    // DTO for returning todo item data
    public record TodoResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
        public int PercentComplete { get; set; }
    }
}
