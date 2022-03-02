using Epico.Entity;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class SprintController : BaseController
    {
        public SprintController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region Index

        public async Task<IActionResult> Index([FromQuery] bool noneError, [FromQuery] bool sprintError)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            // todo sprints[i].Features пустой список
            var sprints = await SprintService.GetAll();
            return View(new SprintViewModel
            {
                SprintError = sprintError,
                NoneError = noneError,
                Sprints = sprints
            });
        }

        #endregion

        #region New

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var model = await GetNewSprintViewModel();
            // Нельзя создать спринт если в проекте нет ни одной фичи
            // todo добавить сообщение
            if (model.PosibleFeatures == null || model.PosibleFeatures.Count == 0)
                return RedirectToAction("Index", "Sprint", new { noneError = true });

            return View(model);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> New(NewSprintViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var remodel = await GetNewSprintViewModel();
                // Нельзя создать спринт если в проекте нет ни одной фичи
                // todo добавить сообщение
                if (remodel.PosibleFeatures == null || remodel.PosibleFeatures.Count == 0)
                    return RedirectToAction("Index", "Sprint");

                return View(remodel);
            }

            if (model.Features == null || model.Features.Count == 0)
            {
                return RedirectToAction("Index", "Sprint", new { sprintError = true });
            }
            var features = await FeatureService.GetByIds(model.Features);

            await ProductService.AddSprint(Product.ID, new Sprint
            {
                Name = model.Name,
                Features = features,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            });
            return RedirectToAction("Index");
        }

        public async Task<NewSprintViewModel> GetNewSprintViewModel()
        {
            var allFeatures = await FeatureService.GetAll();
            var possibleFeatures = allFeatures.Where(x => x.State != FeatureState.None && x.State != FeatureState.Rejected)
                                             .ToList();

            return new NewSprintViewModel
            {
                PosibleFeatures = possibleFeatures
            };
        }

        #endregion

        #region Edit

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            return View(await GetEditSprintViewModel(id));
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditSprintViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetEditSprintViewModel(model.SprintId));

            // todo fix adding features
            var features = await FeatureService.GetByIds(model.Features);
            var sprint = await SprintService.GetById(model.SprintId);
            sprint.Name = model.Name;
            sprint.Features = features;
            sprint.StartDate = model.StartDate;
            sprint.EndDate = model.EndDate;

            await SprintService.Update(sprint);
            return RedirectToAction("Index", "Sprint");
        }

        private async Task<EditSprintViewModel> GetEditSprintViewModel(int sprintId)
        {
            var sprint = await SprintService.GetById(sprintId);
            return new EditSprintViewModel
            {
                SprintId = sprint.ID,
                Name = sprint.Name,
                Features = sprint.Features.Select(x => x.ID).ToList(),
                PosibleFeatures = (await FeatureService.GetAll())
                                  .OrderBy(x => x.IsFeature)
                                  .ToList(),
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate
            };
        }

        #endregion

        #region Delete

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            await SprintService.Delete(id);
            return RedirectToAction("Index", "Sprint");
        }

        #endregion

        #region AddFeature

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> AddFeature(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var sprint = await SprintService.GetById(id);
            var features = await FeatureService.GetAll();
            return View(new AddFeatureToSprintViewModel
            {
                SprintName = sprint.Name,
                SprintId = sprint.ID,
                PosibleFeatures = features.Where(feature => !sprint.Features.Contains(feature)).ToList()
            });
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AddFeature(AddFeatureToSprintViewModel model)
        {
            var features = await FeatureService.GetByIds(model.FeatureIds);
            var sprint = await SprintService.GetById(model.SprintId);

            if (features == null)
                return NotFound("Feature not found");

            sprint.Features.AddRange(features);
            await SprintService.Update(sprint);

            return RedirectToAction("Index");
        }

        #endregion

        #region DeleteFeature

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> DeleteFeature(int id, int sprintId)
        {
            var feature = await FeatureService.GetById(id);
            if (feature == null)
                return BadRequest("Фича/Гипотеза не найдена.");

            var sprint = await SprintService.GetById(sprintId);
            if (!sprint.Features.Contains(feature))
                return BadRequest("Спринт не содержит эту фичу/гипотезу.");

            sprint.Features.Remove(feature);
            await SprintService.Update(sprint);
            return RedirectToAction("Index");
        }

        #endregion
    }
}
