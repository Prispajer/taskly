using Microsoft.EntityFrameworkCore;
using Taskly.Domain.Entities;
using Taskly.Infrastructure.Configurations;

public class TasklyDbContext(DbContextOptions<TasklyDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new TodoConfiguration().Configure(modelBuilder.Entity<Todo>());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
