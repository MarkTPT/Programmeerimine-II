using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using AutoMapper;
using KooliProjekt.Models;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class StoragesController : BaseController
    {
        private readonly IStorageService _storageService;

        public StoragesController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        // GET: Storages
        public async Task<IActionResult> Index(int page)
        {
            var model = await _storageService.StorageList(page);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Storages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _storageService.GetForDetail(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Storages/Create

        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Storages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("StorageID,Kood,SongId")] Storage storage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _uow.Storage.Save(storage);
        //        await _uow.CompleteAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(storage);
        //}

        // GET: Storages/Edit/5

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var storage = await _uow.Storage.Get(id.Value);

            
        //    if (storage == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(storage);
        //}

        // POST: Storages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("StorageID,Kood,SongId")] Storage storage)
        //{
        //    if (id != storage.StorageID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await _uow.Storage.Save(storage);
        //            await _uow.CompleteAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!await StorageExists(storage.StorageID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["SongId"] = new SelectList(_context.Songs, "SongId", "SongId", storage.SongId);
        //    return View(storage);
        //}

        // GET: Storages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storage = await _storageService.GetForDelete(id.Value);
            if (storage == null)
            {
                return NotFound();
            }

            return View(storage);
        }

        // POST: Storages/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _storageService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StorageExists(int id)
        {
            var storage = await _storageService.GetForDelete(id);
            return (storage != null);
        }
    }
}
