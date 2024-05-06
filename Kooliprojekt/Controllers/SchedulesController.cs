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
    public class SchedulesController : BaseController
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // GET: Schedules
        public async Task<IActionResult> Index(int page)
        {
            var model = await _scheduleService.List(page);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _scheduleService.GetForDetail(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            var model = _scheduleService.GetForCreate();
            return View(model);
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ScheduleId,Date,SongId")] ScheduleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _scheduleService.Create(model);
                if (!response.Success)
                {
                    AddModelErrors(response);

                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _scheduleService.GetForEdit(id.Value);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,Date")] ScheduleEditModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (id != model.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _scheduleService.Save(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ScheduleExists(model.ScheduleId))
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

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _scheduleService.GetForDelete(id.Value);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _scheduleService.GetForDelete(id);
            if (schedule == null)
            {
                return NotFound();
            }

            var response = await _scheduleService.Delete(id);
            if (!response.Success)
            {
                AddModelErrors(response);

                return BadRequest(response);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ScheduleExists(int id)
        {
            var schedule = await _scheduleService.GetForDelete(id);
            return (schedule != null);
        }
    }
}
