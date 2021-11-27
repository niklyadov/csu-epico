using Epico.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    public class RoadMapController : BaseController
    {
        public RoadMapController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add(Entity.RoadmapType roadmapType)
        {
            return View(new AddRoadmapViewModel
            {
                Roadmap = roadmapType,
                Features = (await FeatureService.GetFeaturesList())
                    .Where(x=> x.Roadmap != roadmapType).ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddRoadmapViewModel model)
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
