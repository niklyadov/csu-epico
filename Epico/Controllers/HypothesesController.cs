using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;

namespace Epico.Controllers
{
    [Authorize]
    public class HypothesesController : BaseController
    {
        public HypothesesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(new FeatureViewModel
            {
                Features = await FeatureService.GetAll()
            });
        }
    }
}
