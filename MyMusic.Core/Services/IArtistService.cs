using MyMusic.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetAllWithMusic();
        Task<Artist> GetArtistById(int id);

        Task<Artist> CreateArtist(Artist artiste);
        Task<Artist> UpdateArtist(Artist artistToBeUpdated, Artist artist);
        Task DeleteArtist(Artist artist);
    }
}
