using Epico.Entity;
using Epico.Models;
using Epico.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                PossibleUsers = await UserService.GetUsersList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> New(NewTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new NewTaskViewModel
                {
                    PossibleUsers = await UserService.GetUsersList()
                });
            }

            if (model.Users == null)
            {
                return RedirectToAction("Index", "Task", new { error = true });
            }
            var team = await UserService.GetUsersListByIds(model.Users);
            await TaskService.AddTask(model.Name, model.Description, team, model.DeadLine);
            return RedirectToAction("Index", "Task");
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

            var team = await UserService.GetUsersListByIds(model.Users);
            var task = await TaskService.GetById(model.TaskId);
            task.Name = model.Name;
            task.Description = model.Description;
            task.Team = team;
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
                Users = task.Team.Select(x => x.Id).ToList(),
                PosibleUsers = await UserService.GetUsersList()
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
