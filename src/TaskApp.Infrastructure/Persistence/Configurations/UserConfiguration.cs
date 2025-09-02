using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskApp.Domain.Entities;

namespace TaskApp.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Map Email VO
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                     .IsRequired()
                     .HasColumnName("Email")
                     .HasMaxLength(150);
            });

            // Relationships
            builder.HasMany(u => u.Categories)
                   .WithOne(l => l.User)
                   .HasForeignKey(l => l.UserId);
        }
    }
}
