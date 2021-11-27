using Epico.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    public class RoadMapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new AddRoadmapViewModel
            {
                Roadmap = Entity.RoadmapType.DoAfter
            });
        }
        [HttpPost]
        public IActionResult Add(AddRoadmapViewModel model)
        {
            return StatusCode(418);
        }
    }
}
