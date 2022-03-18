using Epico.Entity;
using Epico.Models;
using Epico.Models.ProductEntity;
using Epico.Models.ProductEntity.Feature;
using Epico.Models.ProductEntity.Hypothesis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemTask = System.Threading.Tasks.Task;

namespace Epico.Controllers
{
    public class ProductEntityController : BaseController
    {
        public ProductEntityController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        #region Index

        public async Task<ProductEntityViewModel> IndexPreparation(bool isFeature)
        {
            List<Feature> list;
            ProductEntityViewModel model;
            if (isFeature)
            {
                list = await FeatureService.GetAllFeatures();
                model = new FeatureViewModel { ElementsList = list };
            }
            else
            {
                list = await FeatureService.GetAllHypotheses();
                model = new HypothesisViewModel { ElementsList = list };
            }
            return model;
        }

        #endregion

        #region New

        protected async Task<NewProductEntityViewModel> NewPreparationGet(bool isFeature)
        {
            var possibleMetrics = await MetricService.GetAll();
            var posibleUsers = await UserService.GetAll();
            NewProductEntityViewModel model;
            if (isFeature)
            {
                model = new NewFeatureViewModel
                {
                    PosibleUsers = posibleUsers,
                    PosibleMetrics = possibleMetrics
                };
            }
            else
            {
                model = new NewHypothesisViewModel
                {
                    PosibleUsers = posibleUsers,
                    PosibleMetrics = possibleMetrics
                };
            }
            return model;
        }

        protected async SystemTask NewPreparationPost(NewProductEntityViewModel model, bool isFeature)
        {
            Metric metric = null;
            if (model.MetricId != 0)
            {
                metric = await MetricService.GetById(model.MetricId);
            }
            var users = await UserService.GetByIds(model.UserIds);

            Feature productEntity = new Feature
            {
                Name = model.Name,
                Description = model.Description,
                Tasks = new List<Entity.Task>(),
                Users = users,
                Metric = metric,
            };
            if (isFeature)
            {
                productEntity.Roadmap = RoadmapType.None;
                productEntity.State = FeatureState.None;
                productEntity.IsFeature = true;
            }
            else
            {
                productEntity.IsFeature = false;
            }

            await FeatureService.Add(productEntity);
        }

        #endregion

        #region Edit

        [Authorize(Roles = "Manager")]
        [HttpGet]
        protected async Task<EditProductEntityViewModel> EditPreparationGet(int id, bool isFeature)
        {
            var productEntity = await FeatureService.GetById(id);
            var metricId = productEntity.Metric != null ? productEntity.Metric.ID : 0;
            EditProductEntityViewModel model;
            if (isFeature)
            {
                model = new EditFeatureViewModel { State = productEntity.State };
            }
            else
            {
                model = new EditHypothesisViewModel();
            }
            model.Id = productEntity.ID;
            model.Name = productEntity.Name;
            model.Description = productEntity.Description;
            model.MetricId = metricId;
            model.UserIds = productEntity.Users.Select(x => x.Id).ToList();
            model.PosibleUsers = await UserService.GetAll();
            model.PosibleMetrics = await MetricService.GetAll();
            return model;
        }

        protected async SystemTask EditPreparationPost(EditProductEntityViewModel model, bool isFeature)
        {
            var users = await UserService.GetByIds(model.UserIds);
            var metric = await MetricService.GetById(model.MetricId);
            var productEntity = await FeatureService.GetById(model.Id);

            // Отвязка ответственного если его убрали из команды
            foreach (var item in productEntity.Tasks)
                if (item.ResponsibleUserId.HasValue)
                    if (!model.UserIds.Contains(item.ResponsibleUserId.Value))
                        item.ResponsibleUser = null;

            productEntity.Name = model.Name;
            productEntity.Description = model.Description;
            productEntity.Metric = metric;
            productEntity.Users = users;

            if (isFeature)
            {
                var featureModel = model as EditFeatureViewModel;
                productEntity.State = featureModel.State;
                // Убираем фичу из роадмапа, потому что её статус Rejected
                if (featureModel.State == FeatureState.Rejected)
                    productEntity.Roadmap = RoadmapType.None;
            }
            await FeatureService.Update(productEntity);
        }

        #endregion

        #region Delete


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            await DeletePreparationGet(id);
            return RedirectToAction("Index");
        }

        protected async SystemTask DeletePreparationGet(int id)
        {
            var productEntity = await FeatureService.GetById(id);
            var sprints = (await SprintService.GetAll())
               .Where(sprint => sprint.Features.Contains(productEntity))
               .ToList();
            sprints.ForEach(sprint => sprint.Features.Remove(productEntity));

            await SprintService.UpdateRange(sprints);
            await TaskService.DeleteRange(productEntity.Tasks);
            await FeatureService.Delete(id);
        }

        #endregion

        #region New Task

        [HttpGet]
        public async Task<IActionResult> NewTask(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var model = await NewTaskPreparationGet(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewTask(NewTaskByIdViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await NewTaskPreparationGet(model.ProductEntityId));

            if (model.UserId == 0)
                return RedirectToAction("Index", "Hypothesis", new { taskCreateError = true });

            var productEntity = await FeatureService.GetById(model.ProductEntityId);
            if (!productEntity.Users.Select(x => x.Id).Contains(model.UserId))
                return BadRequest("Юзер не доступен т.к. не содержится в команде гипотезы. Вы чайник.");

            await NewTaskPreparationPost(model);
            return RedirectToAction("Index");
        }

        protected async Task<NewTaskByIdViewModel> NewTaskPreparationGet(int id)
        {
            var productEntity = await FeatureService.GetById(id);
            var model = new NewTaskByIdViewModel
            {
                ProductEntityId = productEntity.ID,
                PossibleUsers = (await UserService.GetAll())
                                .Where(user => productEntity.Users.Contains(user))
                                .ToList()
            };
            return model;
        }

        protected async SystemTask NewTaskPreparationPost(NewTaskByIdViewModel model)
        {
            var productEntity = await FeatureService.GetById(model.ProductEntityId);
            var responsibleUser = await UserService.GetById(model.UserId);
            var task = await TaskService.Add(new Entity.Task
            {
                Name = model.Name,
                Description = model.Description,
                DeadLine = model.DeadLine,
                ResponsibleUser = responsibleUser
            });
            productEntity.Tasks.Add(task);
            await FeatureService.Update(productEntity);
        }

        #endregion

        #region EditTask

        [HttpGet]
        public async Task<IActionResult> EditTask(int id, int taskId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await EditTaskPreparationGet(id, taskId));
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(EditTaskViewModel model)
        {
            if (!ModelState.IsValid)
                return View(await EditTaskPreparationGet(model.ProductEntityId, model.TaskId));

            var responsibleUser = await UserService.GetById(model.UserId);
            var task = await TaskService.GetById(model.TaskId);
            task.Name = model.Name;
            task.Description = model.Description;
            task.ResponsibleUser = responsibleUser;
            task.DeadLine = model.DeadLine;
            task.State = (TaskState)model.State;

            await TaskService.Update(task);

            return RedirectToAction("Index");
        }

        protected async Task<EditTaskViewModel> EditTaskPreparationGet(int productEntityId, int taskId)
        {
            var task = await TaskService.GetById(taskId);
            var productEntity = await FeatureService.GetById(productEntityId);
            var posibleUsers = (await UserService.GetAll())
                               .Where(user => productEntity.Users.Contains(user))
                               .ToList();
            return new EditTaskViewModel
            {
                TaskId = task.ID,
                ProductEntityId = productEntity.ID,
                Name = task.Name,
                Description = task.Description,
                DeadLine = task.DeadLine,
                State = (int)task.State,
                UserId = task.ResponsibleUser != null ? task.ResponsibleUser.Id : 0,
                PosibleUsers = posibleUsers
            };
        }

        #endregion

        #region DeteleTask

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> DeleteTask(int id, int taskId)
        {
            var task = await TaskService.GetById(taskId);
            if (task == null)
                return BadRequest("Задача не найдена.");

            var productEntity = await FeatureService.GetById(id);
            if (!productEntity.Tasks.Contains(task))
                return BadRequest("Гипотеза не содержит эту задачу.");

            productEntity.Tasks.Remove(task);
            await FeatureService.Update(productEntity);
            await TaskService.Delete(task.ID);
            return RedirectToAction("Index");
        }

        #endregion

        #region Show

        [HttpGet]
        public async Task<IActionResult> Show(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var productEntity = await FeatureService.GetById(id);
            return View(productEntity);
        }

        #endregion
    }
}
