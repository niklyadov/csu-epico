using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epico.Entity;
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
            if (!HasProduct) return RedirectToAction("New", "Product");

            var tasks = await TaskService.GetAll();
            return View(new TeamViewModel 
            {
                Tasks = tasks
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddUser(TeamViewModel model)
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            var task = await TaskService.GetById(model.TaskId);
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
        public async Task<IActionResult> AddUser(AddUserToTeamViewModel model)
        {
            var task = await TaskService.GetById(model.TaskId);
            task.Team = await UserService.GetUsersListByIds(model.UserIds);
            await TaskService.Save(task);
            
            return RedirectToAction("Index", "Team");
        }
    }
}
