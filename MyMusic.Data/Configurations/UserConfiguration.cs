using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusic.Core.Models;

namespace MyMusic.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(m => m.Id); // Has a key

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder
                .Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .ToTable("Users");
        }
    }
}
