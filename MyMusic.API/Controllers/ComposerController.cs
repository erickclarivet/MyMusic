using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMusic.API.Resources;
using MyMusic.API.Validation;
using MyMusic.Core.Models;
using MyMusic.Core.Services;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComposerController : ControllerBase
    {
        private readonly IComposerService _composerService;
        private readonly IMapper _mapperService;

        public ComposerController(IComposerService composerService, IMapper mapperService)
        {
            _composerService = composerService;
            _mapperService = mapperService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ComposerResource>>> GetAllComposer()
        {
            try
            {
                var composers = await _composerService.GetAllComposers();

                if (composers == null)
                    return BadRequest("There is no composer.");

                var composersResource = _mapperService.Map<IEnumerable<Composer>, IEnumerable<ComposerResource>>(composers);

                return Ok(composersResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComposerResource>> GetComposerById(string id)
        {
            try
            {
                var composer = await _composerService.GetComposerById(id);
                if (composer == null)
                    return NotFound();

                // We use a mapper in order to show an model to the client not just the DB row
                var composerResource = _mapperService.Map<Composer, ComposerResource>(composer);
                return Ok(composerResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<ActionResult<ComposerResource>> CreateComposer(SaveComposerResource saveComposerResource)
        {
            try
            {
                var validation = new SaveComposerResourceValidator();
                var validationResult = await validation.ValidateAsync(saveComposerResource);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var composerToCreate = _mapperService.Map<SaveComposerResource, Composer>(saveComposerResource);

                var composerCreated = await _composerService.Create(composerToCreate);

                var composerResource = _mapperService.Map<Composer, ComposerResource>(composerCreated);
                return Ok(composerResource);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ComposerResource>> UpdateComposer(string id, SaveComposerResource saveComposerResource)
        {
            try
            {
                var validation = new SaveComposerResourceValidator();
                var validationResult = await validation.ValidateAsync(saveComposerResource);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var composeToBeUpdated = await _composerService.GetComposerById(id);
                if (composeToBeUpdated == null)
                    return BadRequest("Cannot update this composer because it doesn't exist");

                var composer = _mapperService.Map<SaveComposerResource, Composer>(saveComposerResource);

                // Update in DB
                _composerService.Update(id, composer);
                var composerResourceUpdated = _mapperService.Map<Composer, ComposerResource>(composer);
                return Ok(composerResourceUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComposer(string id)
        {
            try
            {
                var composeToDelete = await _composerService.GetComposerById(id);
                if (composeToDelete == null)
                    return BadRequest("Cannot delete this composer because it doesn't exist");

                await _composerService.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
