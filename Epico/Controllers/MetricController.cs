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
    public class MetricController : BaseController
    {
        public MetricController(IServiceProvider serviceProvider) :base(serviceProvider)
        {
        }
        public async Task<IActionResult> Index([FromQuery] bool error, [FromQuery] bool parenterror, [FromQuery] bool deleteMetricError)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var metric = await MetricService.GetNsmMetric();
            if (metric == null)
            {
                return RedirectToAction("New");
            }
            
            return View(new MetricViewModel
            {
                Error = error,
                ParentError = parenterror,
                DeleteMetricError = deleteMetricError,
                Metric = metric
            });
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetNewMetricViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(NewMetricViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetNewMetricViewModel());
            
            var metric = new Metric
            {
                Name = model.Name,
                Description = model.Description
            };
            var possibleParentMetrics = await MetricService.GetAll();
            if (possibleParentMetrics?.Count != 0)
            {
                if (!model.ParentMetricId.HasValue) // Ошибка
                {
                    return RedirectToAction("Index", "Metric", new { error = true });
                }
                    
                metric.ParentMetricId = model.ParentMetricId.Value;

                var parentMetricNew = await MetricService.GetById(model.ParentMetricId.Value);
                parentMetricNew.Children.Add(metric);
                await MetricService.Update(parentMetricNew);
                return RedirectToAction("Index", "Metric");
            }
            await MetricService.Add(metric);
            return RedirectToAction("Index", "Metric");
        }

        private async Task<NewMetricViewModel> GetNewMetricViewModel()
        {
            return new NewMetricViewModel 
            { 
                PosibleParentMetrics = await MetricService.GetAll() 
            };
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetEditMetricViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMetricViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetEditMetricViewModel(model.MetricId));

            var metric = await MetricService.GetById(model.MetricId);
            if (!metric.IsNSM)
            {
                if (metric.Children.Select(x => x.ID).Contains(model.ParentMetricId.Value) || metric.ID == model.ParentMetricId)
                {
                    return RedirectToAction("Index", new { parenterror = true });
                }
            }

            metric.Name = model.Name;
            metric.Description = model.Description;
            metric.ParentMetricId = model.ParentMetricId;

            if (metric.ParentMetricId.HasValue) // удаляем старого child из родителя
            {
                var parentMetricOld = await MetricService.GetById(metric.ParentMetricId.Value);
                    parentMetricOld.Children.Remove(metric);
                await MetricService.Update(parentMetricOld);
            }

            if (model.ParentMetricId.HasValue) // добавляем новый child из родителя
            {
                var parentMetricNew = await MetricService.GetById(model.ParentMetricId.Value);
                    parentMetricNew.Children.Add(metric);
                await MetricService.Update(parentMetricNew);
            }

            await MetricService.Update(metric);
            
            return RedirectToAction("Index", "Metric");
        }

        private async Task<EditMetricViewModel> GetEditMetricViewModel(int metricId)
        {
            var metric = await MetricService.GetById(metricId);
            var possibleParentMetrics = await MetricService.GetAll();
            return new EditMetricViewModel
            {
                MetricId = metric.ID,
                Name = metric.Name,
                Description = metric.Description,
                ParentMetricId = metric.ParentMetricId,
                PosibleParentMetrics = possibleParentMetrics,
                IsNSM = metric.IsNSM
            };
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var metric = await MetricService.GetById(id);
            if (metric.IsNSM) return BadRequest("Нульзя удалить NSM метрику");
            if (metric.Children.Count > 0) return RedirectToAction("Index", new { deleteMetricError = true });

            await MetricService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
