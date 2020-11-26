using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MyMusic.Core.Models;
using MyMusic.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMusic.Data
{
    public class MyMusicDbContext : DbContext
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<Artist> Artists { get; set; }

        public DbSet<User> Users { get; set; }

        public MyMusicDbContext(DbContextOptions<MyMusicDbContext> options)
            : base(options)
        { }

        // We override OnModelCreating in order to apply our configuration
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new MusicConfiguration());

            builder
                .ApplyConfiguration(new ArtistConfiguration());

            builder
                .ApplyConfiguration(new UserConfiguration());
        }

    }
}
