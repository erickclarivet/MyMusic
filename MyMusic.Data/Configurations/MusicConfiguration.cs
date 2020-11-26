using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusic.Core.Models;

namespace MyMusic.Data.Configurations
{
    // Define the configuration of the DB datable Music to convert in SQL (code first i think)
    public class MusicConfiguration : IEntityTypeConfiguration<Music>
    {
        public void Configure(EntityTypeBuilder<Music> builder)
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

            builder // Need to have only one artist and multiple Musics (Foreign key on IdArstist)
                .HasOne(m => m.Artist)
                .WithMany(a => a.Music)
                .HasForeignKey(m => m.IdArtist);

            builder
                .ToTable("Music");
        }
    }
}
