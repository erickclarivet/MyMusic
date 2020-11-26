using MyMusic.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyMusic.Core.Services
{
    public interface IComposerService
    {
        Task<IEnumerable<Composer>> GetAllComposers();

        Task<Composer> GetComposerById(string id);

        Task<Composer> Create(Composer composer);

        Task<bool> Delete(string id);

        void Update(string id, Composer composer);
    }
}
