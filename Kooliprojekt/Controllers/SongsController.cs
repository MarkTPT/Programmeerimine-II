using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Models;
using AutoMapper;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class SongsController : BaseController
    {
        private readonly ISongService _songService;

        public SongsController(ISongService songService)
        {
            _songService = songService;
        }
        
        // GET: Songs
        public async Task<IActionResult> Index(int page)
        {
            var model = await _songService.List(page);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _songService.GetForDetail(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Songs/Create
        public async Task<IActionResult> Create()
        {
            var model = await _songService.GetForCreate();

            return View(model);
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("SongId,Title,Tempo,Artist,ArtistId,Storage")] SongCreationModel model)
        {
            if (ModelState.IsValid)
            {
                await _songService.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _songService.GetForEdit(id.Value);

            if (model == null)
            {
                return NotFound();
            }


            return View(model);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("SongId,Title,Tempo,ArtistId")] SongEditModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (id != model.SongId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                try
                {
                    await _songService.Edit(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SongExists(model.SongId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _songService.GetForDelete(id.Value);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _songService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SongExists(int id)
        {
            var song = await _songService.GetForDelete(id);
            return (song != null);
        }
    }
}
