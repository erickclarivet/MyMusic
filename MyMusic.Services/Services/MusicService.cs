using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using MyMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services.Services
{
    public class MusicService : IMusicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MusicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Music> CreateMusic(Music music)
        {
            await _unitOfWork.Musics.AddAsync(music);
            await _unitOfWork.CommitAsync();

            return music;
        }

        public async Task DeleteMusic(Music music)
        {
            _unitOfWork.Musics.Remove(music);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Music> UpdateMusic(Music musicToBeUpdated, Music music)
        {
            musicToBeUpdated.Name = music.Name;
            musicToBeUpdated.IdArtist = music.IdArtist;

            await _unitOfWork.CommitAsync();

            return musicToBeUpdated;
        }

        public async Task<IEnumerable<Music>> GetAllWithArtist()
        {
            return await _unitOfWork.Musics.GetAllWithArtistAsync();
        }

        public async Task<IEnumerable<Music>> GetMusicByArtistId(int artistId)
        {
            return await _unitOfWork.Musics
                .GetAllWithArtistByIdAsync(artistId);
        }

        public async Task<Music> GetMusicById(int id)
        {
            return await _unitOfWork.Musics
                 .GetByIdAsync(id);
        }
    }
}
