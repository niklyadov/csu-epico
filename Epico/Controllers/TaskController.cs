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
    public class TaskController : BaseController
    {
        public TaskController(IServiceProvider serviceProvider):base(serviceProvider)
        {
            
        }
        public async Task<IActionResult> Index([FromQuery] bool error)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var tasks = await TaskService.GetAll();
            return View(new TaskViewModel
            {
                Error = error,
                Tasks = tasks
            });
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(new NewTaskViewModel 
            {
                PossibleUsers = await UserService.GetAll()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new NewTaskViewModel
                {
                    PossibleUsers = await UserService.GetAll()
                });
            }

            if (model.UserId == 0)
            {
                return RedirectToAction("Index", "Task", new { error = true });
            }
            var responsibleUser = await UserService.GetById(model.UserId);
            await TaskService.Add(new Entity.Task
            {
                Name = model.Name,
                Description = model.Description,
                DeadLine = model.DeadLine,
                ResponsibleUser = responsibleUser
            });
            return RedirectToAction("Index", "Task");
        }

        [HttpGet]
        public async Task<IActionResult> NewById([FromQuery] int featureId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");
            var feature = await FeatureService.GetById(featureId);
            return View(new NewTaskByIdViewModel
            {
                FeatureId = featureId,
                PossibleUsers = (await UserService.GetAll())
                                .Where(user => feature.Users.Contains(user))
                                .ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> NewById(NewTaskByIdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new NewTaskByIdViewModel
                {
                    FeatureId = model.FeatureId,
                    PossibleUsers = await UserService.GetAll()
                });
            }

            if (model.UserId == 0)
            {
                return RedirectToAction("Index", "Feature", new { taskCreateError = true });
            }

            var feature = await FeatureService.GetById(model.FeatureId);
            if (!feature.Users.Select(x => x.Id).Contains(model.UserId))
            {
                return BadRequest("Юзер не доступен т.к. не содержится в команде фичи. Вы чайник.");
            }

            var responsibleUser = await UserService.GetById(model.UserId);
            var task = await TaskService.Add(new Entity.Task
            {
                Name = model.Name,
                Description = model.Description,
                DeadLine = model.DeadLine,
                ResponsibleUser = responsibleUser
            });
            feature.Tasks.Add(task);
            await FeatureService.Update(feature);
            return RedirectToAction("Index", "Feature");
        }









        [HttpGet]
        public async Task<IActionResult> Edit(TaskViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetEditTaskViewModel(model.TaskId));
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(EditTaskViewModel model)
        {
            if (!ModelState.IsValid) return View(await GetEditTaskViewModel(model.TaskId));

            var responsibleUser = await UserService.GetById(model.UserId);
            var task = await TaskService.GetById(model.TaskId);
            task.Name = model.Name;
            task.Description = model.Description;
            task.ResponsibleUser = responsibleUser;
            task.DeadLine = model.DeadLine;
            task.State = (TaskState)model.State;
            
            await TaskService.Update(task);

            return RedirectToAction("Index", "Task");
        }

        private async Task<EditTaskViewModel> GetEditTaskViewModel(int taskId)
        {
            var task = await TaskService.GetById(taskId);
            return new EditTaskViewModel
            {
                TaskId = task.ID,
                Name = task.Name,
                Description = task.Description,
                DeadLine = task.DeadLine,
                State = (int)task.State,
                UserId = task.ResponsibleUser.Id,
                PosibleUsers = await UserService.GetAll()
            };
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] int taskId)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            if (!ModelState.IsValid) return BadRequest("ModelState is not Valid");

            await TaskService.Delete(taskId);
            return RedirectToAction("Index", "Task");
        }

        [HttpGet]
        public async Task<IActionResult> EditState(TaskViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var task = await TaskService.GetById(model.TaskId);
            return View(new EditStateTaskViewModel
            {
                TaskId = task.ID,
                Task = task
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditState(EditStateTaskViewModel model)
        {
            var task = await TaskService.GetById(model.TaskId);

            task.State = model.State;
            
            await TaskService.Update(task);
            
            return RedirectToAction("Index", "Task");
        }
    }
}
