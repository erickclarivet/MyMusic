using MyMusic.Core.Models;
using MyMusic.Core.Repositories;
using MyMusic.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Services.Services
{
    public class ComposerService : IComposerService
    {
        private readonly IComposerRepository _composer;
        
        public ComposerService(IComposerRepository composer)
        {
            _composer = composer;
        }

        public async Task<Composer> Create(Composer composer)
        {
            return await _composer.Create(composer); 
        }

        public async Task<bool> Delete(string id)
        {
            return await _composer.Delete(id);
        }

        public async Task<IEnumerable<Composer>> GetAllComposers()
        {
            return await _composer.GetAllComposers();
        }

        public async Task<Composer> GetComposerById(string id)
        {
            return await _composer.GetComposerById(id);
        }

        public void Update(string id, Composer composer)
        {
           _composer.Update(id, composer);
        }
    }
}
