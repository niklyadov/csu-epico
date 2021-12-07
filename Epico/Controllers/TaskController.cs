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
        public async Task<IActionResult> Edit(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            return View(await GetEditTaskViewModel(id));
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

            var feature = (await FeatureService.GetAll()).Where(feature => feature.Tasks.Contains(task)).First();
            var posibleUsers = (await UserService.GetAll()).Where(user => feature.Users.Contains(user)).ToList();
            var responsibleUserId = task.ResponsibleUser != null ? task.ResponsibleUser.Id : 0;
            return new EditTaskViewModel
            {
                TaskId = task.ID,
                Name = task.Name,
                Description = task.Description,
                DeadLine = task.DeadLine,
                State = (int)task.State,
                UserId = responsibleUserId,
                PosibleUsers = posibleUsers
            };
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            await TaskService.Delete(id);
            return RedirectToAction("Index", "Task");
        }

        [HttpGet]
        public async Task<IActionResult> EditState(int id)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var task = await TaskService.GetById(id);
            return View(new EditStateTaskViewModel
            {
                TaskId = task.ID,
                Task = task
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditState(EditStateTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var retask = await TaskService.GetById(model.TaskId);
                return View(new EditStateTaskViewModel
                {
                    TaskId = retask.ID,
                    Task = retask
                });
            }
            var task = await TaskService.GetById(model.TaskId);
            task.State = model.State;
            await TaskService.Update(task);
            return RedirectToAction("Index", "Task");
        }
    }
}
