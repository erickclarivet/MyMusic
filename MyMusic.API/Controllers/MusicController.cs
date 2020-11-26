using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Resources;
using MyMusic.API.Validation;
using MyMusic.Core.Models;
using MyMusic.Core.Services;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly IMapper _mapperService;

        public MusicController(IMusicService musicService, IMapper mapperService)
        {
            _musicService = musicService;
            _mapperService = mapperService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusics()
        {
            try
            {
                var musics = await _musicService.GetAllWithArtist();
                // We use a mapper in order to show an model to the client not just the DB row
                var musicResources = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
                return Ok(musicResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<MusicResource>> GetMusicById(int id)
        {
            try
            {
                var music = await _musicService.GetMusicById(id);
                if (music == null)
                    return NotFound();

                // We use a mapper in order to show an model to the client not just the DB row
                var musicResource = _mapperService.Map<Music, MusicResource>(music);
                return Ok(musicResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Artist/{id}")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusicByIdArtist(int id)
        {
            try
            {
                var musics = await _musicService.GetMusicByArtistId(id);
                if (musics == null)
                    return BadRequest("There are no music for this artist.");

                // We use a mapper in order to show an model to the client not just the DB row
                var musicsResource = _mapperService.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
                return Ok(musicsResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        [Authorize] // We need to be authenticate to create a music
        public async Task<ActionResult<MusicResource>> CreateMusic(SaveMusicResource saveMusicResource)
        {
            try
            {
                // Validation
                var validation = new SaveMusicResourceValidator();
                var validationResult = await validation.ValidateAsync(saveMusicResource);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);
                
                // Mappage
                var music = _mapperService.Map<SaveMusicResource, Music>(saveMusicResource);
                
                // Music Creation
                var newMusic = await _musicService.CreateMusic(music);

                // Mappage
                var musicResource = _mapperService.Map<Music, MusicResource>(newMusic);
                return Ok(musicResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MusicResource>> UpdateMusic(int id, SaveMusicResource saveMusicResource)
        {
            try
            {
                var validation = new SaveMusicResourceValidator();
                var validationResult = await validation.ValidateAsync(saveMusicResource);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var musicToBeUpdated = await _musicService.GetMusicById(id);
                if (musicToBeUpdated == null)
                    return BadRequest("Cannot update this music because it doesn't exist");

                var music = _mapperService.Map<SaveMusicResource, Music>(saveMusicResource);

                // Update in DB
                var musicUpdated = await _musicService.UpdateMusic(musicToBeUpdated, music);
                var musicResourceUpdated = _mapperService.Map<Music, MusicResource>(musicUpdated);
                return Ok(musicResourceUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMusic(int id)
        {
            try
            {
                var music = await _musicService.GetMusicById(id);

                if (music == null)
                    return BadRequest("The music doesn't exist.");

                await _musicService.DeleteMusic(music);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
