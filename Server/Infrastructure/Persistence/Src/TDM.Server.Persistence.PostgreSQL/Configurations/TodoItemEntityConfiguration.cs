using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDM.Api.Enum;
using TDM.Domain.Entities;

namespace TDM.Server.Persistence.PostgreSQL.Configurations;

public class TodoItemEntityConfiguration : IEntityTypeConfiguration<TodoItemEntity>
{
    public void Configure(EntityTypeBuilder<TodoItemEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Details);

        builder.Property(e => e.DueDate);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasDefaultValue(TodoStatus.NotStarted);

        builder.Property(e => e.Priority)
            .IsRequired()
            .HasDefaultValue(Priority.NotSpecified);

        builder.Property(e => e.ContactId);

        builder.Property(e => e.CompletedAt);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired();
        
        builder.HasIndex(e => e.Title);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Priority);
        builder.HasIndex(e => e.ContactId);
        
        builder.HasOne(e => e.Contact)
            .WithMany(c => c.TodoItems)
            .HasForeignKey(e => e.ContactId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.ToTable("TodoItems");
    }
}
