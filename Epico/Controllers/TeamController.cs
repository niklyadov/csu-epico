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
        public async Task<IActionResult> Add(TeamViewModel model)
        {
            // todo проверки на нуллы и т.д.
            var task = await TaskService.GetTaskById(model.TaskId);
            var users = await UserService.GetUsersList();
            return View(new AddTeamViewModel 
            {
                TaskId = task.ID,
                TaskName = task.Name,
                Users = users.Where(x => !task.Team.Contains(x)).ToList()
            });
        }

        [HttpPost]
        public IActionResult Add(AddTeamViewModel model)
        {
            // todo сохранить в базу изменения команды у задачи
            return RedirectToAction("Index", "Team");
        }
    }
}
