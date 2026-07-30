using Microsoft.EntityFrameworkCore;
using Taskly.Application.Abstractions.Data;
using Taskly.Domain.Todos.Entities;
using Taskly.Infrastructure.Configuration;

namespace Taskly.Infrastructure.Persistence;
public class TasklyDbContext(DbContextOptions<TasklyDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Todo> Todos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new TodoConfiguration().Configure(modelBuilder.Entity<Todo>());
    }
}
