using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epico.Models;

namespace Epico.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return StatusCode(418, "I,m a teapot");
        }

        [HttpPost]
        public IActionResult Add(AddTeamViewModel model)
        {
            return StatusCode(418, "I,m a teapot");
        }
    }
}
