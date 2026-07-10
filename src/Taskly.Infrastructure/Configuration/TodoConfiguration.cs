using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskly.Domain.Todos.Entities;

namespace Taskly.Infrastructure.Configuration;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("Todos"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
        builder.OwnsOne(t => t.Expiry, e =>
        {
            e.Property(x => x.Value)
                .HasColumnName("Expiry")
                .HasColumnType("timestamp")
                .IsRequired();
        });
        builder.OwnsOne(t => t.PercentComplete, pc =>
        {
            pc.Property(x => x.Value)
                .HasColumnName("PercentComplete")
                .HasColumnType("integer")
                .HasDefaultValue(0);
        });
        builder.Ignore(x => x.DomainEvents);
    }
}

