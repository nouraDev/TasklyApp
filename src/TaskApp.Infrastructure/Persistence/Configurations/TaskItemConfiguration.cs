using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskApp.Domain.Entities;

namespace TaskApp.Infrastructure.Persistence.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("TaskItems");

            builder.HasKey(t => t.Id);

            builder.OwnsOne(t => t.Title, title =>
            {
                title.Property(ti => ti.Value)
                     .IsRequired()
                     .HasMaxLength(200)
                     .HasColumnName("Title");
            });
            builder.OwnsOne(t => t.DueDate, dueDate =>
            {
                dueDate.Property(dd => dd.Value)
                       .IsRequired();
            });

            builder.Property(t => t.Description)
                   .HasMaxLength(1000);

            builder.Property(t => t.IsCompleted);

            builder.Property(t => t.CreatedAt)
                   .IsRequired();

            builder.Property(t => t.CompletedAt);

            builder.Property(t => t.PriorityCategory)
                   .HasConversion<int>()
                   .IsRequired();

            // Relationships
            builder.HasOne(t => t.Category)
                   .WithMany(l => l.Tasks)
                   .HasForeignKey(t => t.CategoryId);

            builder.HasIndex(t => t.CategoryId);
            builder.HasIndex(t => t.IsCompleted);
            builder.HasIndex(t => t.CreatedAt);
            builder.HasIndex(t => t.CompletedAt);
        }
    }
}
