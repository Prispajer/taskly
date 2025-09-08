using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskly.Domain.Todos;

namespace Taskly.Infrastructure.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.ToTable("Todos"); builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Expiry).HasColumnType("timestamp").IsRequired().HasConversion(
        v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified),
        v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified));
        builder.Property(x => x.PercentComplete).HasDefaultValue(0);
    }
}

