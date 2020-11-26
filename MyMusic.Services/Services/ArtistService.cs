using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using MyMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArtistService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Artist> CreateArtist(Artist artist)
        {
            await _unitOfWork.Artists.AddAsync(artist);
            await _unitOfWork.CommitAsync();

            return artist;
        }

        public async Task DeleteArtist(Artist artist)
        {
            _unitOfWork.Artists.Remove(artist);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Artist>> GetAllWithMusic()
        {
            return await _unitOfWork.Artists.GetAllWithMusicAsync();
        }

        public async Task<Artist> GetArtistById(int id)
        {
            return await _unitOfWork.Artists.GetByIdAsync(id);
        }

        public async Task<Artist> UpdateArtist(Artist artistToBeUpdated, Artist artist)
        {
            artistToBeUpdated.Name = artist.Name;
            artistToBeUpdated.Music = artist.Music;

            await _unitOfWork.CommitAsync();
            return artistToBeUpdated;
        }
    }
}
