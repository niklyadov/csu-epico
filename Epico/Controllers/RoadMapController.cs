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
    }
}
