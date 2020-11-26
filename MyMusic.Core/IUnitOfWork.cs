using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core.Repositories
{
    // Follows a list of update and apply a commit of the modifications at the end in async way
    public interface IUnitOfWork : IDisposable
    {
        IArtistRepository Artists { get; }
        IMusicRepository Musics { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
