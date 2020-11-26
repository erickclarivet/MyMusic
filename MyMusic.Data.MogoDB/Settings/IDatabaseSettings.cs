using MongoDB.Driver;
using MyMusic.Core.Models;

namespace MyMusic.Data.MogoDB.Settings
{
    public interface IDatabaseSettings
    {
        IMongoCollection<Composer> Composers { get; }
    }
}
