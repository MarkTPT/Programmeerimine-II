using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Http;
using KooliProjekt.Services;
using KooliProjekt.Data;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Controllers
{
    public class ArtistsController : BaseController
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        // GET: Artists
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _artistService.List(page);

            if (model == null)
            {
                return NotFound();
            }

            //ViewBag.Artists = new SelectList(_context.Artists, "ArtistId", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<IFormFile> formFiles, string artistId, int page)
        {
            if (formFiles == null)
            {
                return BadRequest();
            }

            var model = await _artistService.SaveFile(formFiles, artistId, page);


            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        public async Task<IActionResult> About()
        {
            var model = await _artistService.GetForAbout();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> About(string container, string fileName)
        {
            var model = await _artistService.DeleteFile(container, fileName);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [ExcludeFromCodeCoverage]
        public async Task<IActionResult> AboutTest()
        {
            var model = await _artistService.GetForAbout();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _artistService.GetForDetail(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(ArtistModel model)
        {

            //if (ModelState.IsValid)
            //{
            //    await _artistService.Create(model);
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(model);

            if (model == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _artistService.Save(model);
                    if (!response.Success)
                    {
                        AddModelErrors(response);

                        return View(model);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ArtistExists(model.ArtistId))
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

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            

            if (id == null)
            {
                return NotFound();
            }

            var model = await _artistService.GetForEdit(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        //private void PopulateArtistDropdownList(object selectedSong)

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistId,Name,Description")] ArtistModel model)
        {
            if(model == null)
            {
                return BadRequest();
            }

            if (id != model.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _artistService.Save(model);
                    if (!response.Success)
                    {
                        AddModelErrors(response);

                        return View(model);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ArtistExists(model.ArtistId))
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


        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _artistService.GetForDelete(id.Value);

            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _artistService.GetForDelete(id);
            if (artist == null)
            {
                return NotFound();
            }

            var response = await _artistService.Delete(id);
            if (!response.Success)
            {
                AddModelErrors(response);

                return BadRequest(response);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ArtistExists(int id)
        {
            var artist = await _artistService.GetForDelete(id);
            return (artist != null);
        }

    }
}
