using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using KooliProjekt.Models;
using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var list = await _artistService.ApiGetList();

            string json = JsonConvert.SerializeObject(list.Results, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore

            });


            return json;
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostArtist(ArtistModel model)
        {
            var response = await _artistService.Save(model);

            return CreatedAtAction(nameof(GetArtist), new { id = model.ArtistId }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetArtist(int id)
        {
            if(id == 0)
            {
                return null;
            }
            var artist = await _artistService.GetForDetail(id);

            if(artist == null)
            {
                return NotFound();
            }

            string json = JsonConvert.SerializeObject(artist, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return json;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist([FromRoute]int id, ArtistModel artist)
        {
            if (id != artist.ArtistId)
            {
                return BadRequest();
            }

            //_artistService.ChangeEntityStateTo(artist, EntityState.Modified);

            try
            {
                var response = await _artistService.Save(artist);
                if (!response.Success)
                {
                    return BadRequest(response);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ArtistExists(artist.ArtistId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist([FromRoute]int id)
        {
            var artist = await _artistService.GetForDelete(id);
            if (artist == null)
            {
                return NotFound();
            }

            await _artistService.Delete(id);

            return NoContent();
        }

        private async Task<bool> ArtistExists(int id)
        {
            var artist = await _artistService.GetForDelete(id);
            return (artist != null);
        }
    }
}
