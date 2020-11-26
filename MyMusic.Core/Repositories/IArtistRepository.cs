using MyMusic.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusic.Core.Repositories
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Task<IEnumerable<Artist>> GetAllWithMusicAsync();

        Task<Artist> GetWithMusicByIdAsync(int id);

        Task<IEnumerable<Artist>> GetAllWithMusicByIdAsync(int artistId);
    }
}
