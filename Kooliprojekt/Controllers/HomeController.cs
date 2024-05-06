using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KooliProjekt.Models;
using KooliProjekt.Services;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IScheduleService _scheduleService;

        public HomeController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _scheduleService.GetForHome();

            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ExcludeFromCodeCoverage]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
