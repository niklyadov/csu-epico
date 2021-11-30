using Epico.Entity;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Epico.Controllers
{
    [Authorize]
    public class MetricController : BaseController
    {
        public MetricController(IServiceProvider serviceProvider) :base(serviceProvider)
        {
        }
        public async Task<IActionResult> Index([FromQuery] bool error)
        {
            var product = await ProductService.GetProduct();
            if (product == null)
            {
                return RedirectToAction("New", "Product");
            }

            var metric = await MetricService.GetNsmMetric();
            if (metric == null)
            {
                return RedirectToAction("New");
            }
            
            return View(new MetricViewModel
            {
                Error = error,
                ProductId = product.ID,
                Metric = metric
            });
        }

        [HttpGet]
        public async Task<IActionResult> New(MetricViewModel model)
        {
            return View(new NewMetricViewModel
            {
                ProductId = model.ProductId,
                PosibleParentMetrics = await MetricService.GetMetricList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewMetricViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");
            
            var metric = new Metric
            {
                Name = model.Name,
                Description = model.Description
            };
            var possibleParentMetrics = await MetricService.GetMetricList();
            if (possibleParentMetrics?.Count != 0)
            {
                if (!model.ParentMetricId.HasValue) // Ошибка
                {
                    return RedirectToAction("Index", "Metric", new { error = true });
                }
                    
                metric.ParentMetricId = model.ParentMetricId.Value;

                var parentMetricNew = await MetricService.GetMetricById(model.ParentMetricId.Value);
                parentMetricNew.Children.Add(metric);
                await MetricService.UpdateMetric(parentMetricNew);
                return RedirectToAction("Index", "Metric");
            }
            await MetricService.AddMetric(metric);
            return RedirectToAction("Index", "Metric");
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] int projectId, [FromQuery] int metricId)
        {
            var metric = await MetricService.GetMetricById(metricId);
            var possibleParentMetrics = await MetricService.GetMetricList();
            return View(new EditMetricViewModel
            {
                ID = metric.ID,
                Name = metric.Name,
                Description = metric.Description,
                ParentMetricId = metric.ParentMetricId,
                ProductId = projectId,
                PosibleParentMetrics = possibleParentMetrics
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMetricViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            var metric = await MetricService.GetMetricById(model.ID);

            metric.Name = model.Name;
            metric.Description = model.Description;
            metric.ParentMetricId = model.ParentMetricId;

            if (metric.ParentMetricId.HasValue) // удаляем старого child из родителя
            {
                var parentMetricOld = await MetricService.GetMetricById(metric.ParentMetricId.Value);
                    parentMetricOld.Children.Remove(metric);
                await MetricService.UpdateMetric(parentMetricOld);
            }

            if (model.ParentMetricId.HasValue) // добавляем новый child из родителя
            {
                var parentMetricNew = await MetricService.GetMetricById(model.ParentMetricId.Value);
                    parentMetricNew.Children.Add(metric);
                await MetricService.UpdateMetric(parentMetricNew);
            }

            await MetricService.UpdateMetric(metric);
            
            return RedirectToAction("Index", "Metric");
        }

        public async Task<IActionResult> Delete([FromQuery] int metricId)
        {
            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await MetricService.DeleteMetric(metricId);
            return Ok("Метрика удалена");
        }
    }
}
