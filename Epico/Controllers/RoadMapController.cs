using Epico.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace Epico.Controllers
{
    
    [Authorize]
    public class RoadMapController : BaseController
    {
        public RoadMapController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var features = await FeatureService.GetAllFeatures();
            return View(new RoadMapViewModel
            {
                Features = features
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddFeature(RoadMapViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var features = await FeatureService.GetAllFeatures();
            return View(new AddRoadmapViewModel
            {
                Roadmap = (Entity.RoadmapType)model.Roadmap,
                Features = features.Where(x => x.Roadmap != (Entity.RoadmapType)model.Roadmap).ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddFeature(AddRoadmapViewModel model)
        {
            var feature = await FeatureService.GetById(model.FeatureId);

            if (feature == null)
            {
                return NotFound("Feature not found");
            }

            feature.Roadmap = model.Roadmap;
            await FeatureService.Update(feature);

            return RedirectToAction("Index");
        }
    }
}
