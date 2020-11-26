using AutoMapper;
using MyMusic.API.Resources;
using MyMusic.Core.Models;

namespace MyMusic.API.Mapping
{
    public class MappingProfile : Profile
    {
        
        public MappingProfile()
        {
            // Automapping Db o Resources (map all the prop db to ressource)
            // Possible to map just some prop
            CreateMap<Music, MusicResource>();
            CreateMap<Artist, ArtistResource>();
            CreateMap<Music, SaveMusicResource>();
            CreateMap<Artist, SaveArtistResource>();
            CreateMap<Composer, ComposerResource>();
            CreateMap<Composer, SaveComposerResource>();
            CreateMap<User, UserResource>();

            // Automapping Resources to DB
            CreateMap<MusicResource, Music>();
            CreateMap<ArtistResource, Artist>();
            CreateMap<SaveMusicResource, Music>();
            CreateMap<SaveArtistResource, Artist>();
            CreateMap<ComposerResource, Composer>();
            CreateMap<SaveComposerResource, Composer>();
            CreateMap<UserResource, User>();
        }
    }
}
