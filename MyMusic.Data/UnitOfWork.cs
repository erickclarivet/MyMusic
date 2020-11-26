﻿using MyMusic.Core.Repositories;
using MyMusic.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace MyMusic.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyMusicDbContext _context;
        private IArtistRepository _artistRepository;
        private IMusicRepository _musicRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(MyMusicDbContext context)
        {
            _context = context;
        }

        public IArtistRepository Artists => _artistRepository = _artistRepository ?? new ArtistRepository(_context);
        public IMusicRepository Musics => _musicRepository = _musicRepository ?? new MusicRepository(_context);

        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        { 
            _context.Dispose();
        }
    }
}