using Microsoft.EntityFrameworkCore;
using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public MyMusicDbContext MyMusicDbContext
        {
            get
            {
                return Context as MyMusicDbContext;
            }
        }

        public ArtistRepository(MyMusicDbContext context) 
            : base(context) {}

        public async Task<IEnumerable<Artist>> GetAllWithMusicAsync()
        {
            return await MyMusicDbContext.Artists
                .Include(m => m.Music)
                .ToListAsync();
        }

        public async Task<Artist> GetWithMusicByIdAsync(int id)
        {
            return await MyMusicDbContext.Artists
                .Include(m => m.Music)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Artist>> GetAllWithMusicByIdAsync(int artistId)
        {
            return await MyMusicDbContext.Artists
                .Include(m => m.Music)
                .Where(m => m.Id == artistId)
                .ToListAsync();
        }

    }
}
