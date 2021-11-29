using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epico.Models;

namespace Epico.Controllers
{
    public class TeamController : BaseController
    {
        public TeamController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await TaskService.GetTaskList();
            return View(new TeamViewModel 
            {
                Tasks = tasks
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddUser(TeamViewModel model)
        {
            var task = await TaskService.GetTaskById(model.TaskId);
            if (task == null)
            {
                return BadRequest("Сначала нужно создать хотя бы одну задачу!");
            }
            
            var users = await UserService.GetUsersList();
            return View(new AddUserToTeamViewModel 
            {
                TaskId = task.ID,
                TaskName = task.Name,
                PosibleUsers = users.Where(x => !task.Team.Contains(x)).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddUser(AddUserToTeamViewModel model)
        {
            var task = await TaskService.GetTaskById(model.TaskId);
            task.Team = model.Users;
            await TaskService.UpdateTask(task);
            
            return RedirectToAction("Index", "Team");
        }
    }
}
