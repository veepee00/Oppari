using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oppari.Models;
using Oppari.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Oppari.Controllers
{
    public class WatchDogController : Controller
    {
        //Apuja
        private readonly IHubContext<WatchDogHub> _hubContext;
        public WatchDogController(IHubContext<WatchDogHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            int errorCount;
            using (var context = new WatchDogErrorContext())
            {
                errorCount = context.WatchDogErrors.Count();
            }
            ViewData["ErrorCount"] = errorCount;
            return View();
        }

        public IActionResult WatchDogErrors()
        {
            int errorCount;
            List<WatchDogErrorModel> errorList = new List<WatchDogErrorModel>();
            using (var context = new WatchDogErrorContext())
            {
                errorCount = context.WatchDogErrors.Count();
                errorList = context.WatchDogErrors.Where(e => e.Status == 50).ToList();
            }

            ViewData["ErrorCount"] = errorCount;
            ViewData["ErrorList"] = errorList;
            return View();
        }         
    }
}