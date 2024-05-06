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
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongController(ISongService songService)
        {
            _songService = songService;
        }
        // GET: api/<SongController>
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var list = await _songService.ApiGetList();

            string json = JsonConvert.SerializeObject(list.Results, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return json;
        }

        // GET api/<SongController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetSong(int id)
        {
            var artist = await _songService.GetForDetail(id);

            if (artist == null)
            {
                return NotFound();
            }

            string json = JsonConvert.SerializeObject(artist, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return json;
        }

        // POST api/<SongController>
        [HttpPost]
        public async Task<ActionResult<string>> PostSong(SongCreationModel model)
        {
            var response = await _songService.Create(model);

            return CreatedAtAction(nameof(GetSong), new { id = model.SongId }, model);
        }

        // PUT api/<SongController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SongController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
