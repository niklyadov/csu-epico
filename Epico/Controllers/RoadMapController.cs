using Epico.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    public class RoadMapController : BaseController
    {
        public RoadMapController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<IActionResult> Index()
        {
            var features = await FeatureService.GetFeaturesList();
            return View(new RoadMapViewModel
            {
                Features = features
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddFeature(RoadMapViewModel model)
        {
            var features = await FeatureService.GetFeaturesList();
            return View(new AddRoadmapViewModel
            {
                Roadmap = (Entity.RoadmapType)model.Roadmap,
                Features = features.Where(x => x.Roadmap != (Entity.RoadmapType)model.Roadmap).ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddFeature(AddRoadmapViewModel model)
        {
            var feature = await FeatureService.GetFeature(model.FeatureId);

            if (feature == null)
            {
                return NotFound("Feature not found");
            }

            feature.Roadmap = model.Roadmap;
            await FeatureService.UpdateFeature(feature);

            return RedirectToAction("Index");
        }
    }
}
