using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusic.Core.Models;

namespace MyMusic.Data.Configurations
{
    public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder
                .HasKey(m => m.Id); // Has a key

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder // Id mandatory and has to have max 50 char
                .Property(m => m.Id)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .ToTable("Artist");
        }
    }
}
