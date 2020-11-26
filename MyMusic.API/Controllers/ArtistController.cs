using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Resources;
using MyMusic.API.Validation;
using MyMusic.Core.Models;
using MyMusic.Core.Services;
using MyMusic.Services.Services;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly IMapper _mapperService;

        public ArtistController(IArtistService artistService, IMapper mapperService)
        {
            _artistService = artistService;
            _mapperService = mapperService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ArtistResource>>> GetAllArtists()
        {
            try
            {
                var artists = await _artistService.GetAllWithMusic();

                if (artists == null)
                    return BadRequest("There is no artist.");

                var artistsResources = _mapperService.Map<IEnumerable<Artist>, IEnumerable<ArtistResource>>(artists);
                return Ok(artistsResources);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistResource>> GetArtistById(int id)
        {
            try
            {
                var artist = await _artistService.GetArtistById(id);

                if (artist == null)
                    return BadRequest("This artist doesn't exist.");

                var artistsResource = _mapperService.Map<Artist, ArtistResource>(artist);
                return Ok(artistsResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<ActionResult<ArtistResource>> CreateArtist(SaveArtistResource saveArtistResource)
        {
            try
            {
                var validation = new SaveArtistResourceValidator();
                var validationResult = await validation.ValidateAsync(saveArtistResource);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var artistToCreate = _mapperService.Map<SaveArtistResource, Artist>(saveArtistResource);

                var artistCreated = await _artistService.CreateArtist(artistToCreate);

                var artistsResource = _mapperService.Map<Artist, ArtistResource>(artistCreated);
                return Ok(artistsResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArtistResource>> UpdateArtist(int id, SaveArtistResource saveArtistResource)
        {
            try
            {
                var validation = new SaveArtistResourceValidator();
                var validationResult = await validation.ValidateAsync(saveArtistResource);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var artistToUpdate = await _artistService.GetArtistById(id);
                if (artistToUpdate == null)
                    return BadRequest("Cannot update this artist because it doesn't exist");

                var artist = _mapperService.Map<SaveArtistResource, Artist>(saveArtistResource);

                var artistUpdated = await _artistService.UpdateArtist(artistToUpdate, artist);

                var artistsResourceUpdated = _mapperService.Map<Artist, ArtistResource>(artistUpdated);
                return Ok(artistsResourceUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult>   DeleteArtist(int id)
        {
            try
            {
                var artistToDelete = await _artistService.GetArtistById(id);

                if (artistToDelete == null)
                    return BadRequest("Cannot delete this artist because it doesn't exist.");

                await _artistService.DeleteArtist(artistToDelete);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
