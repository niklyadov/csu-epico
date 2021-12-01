using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;

namespace Epico.Controllers
{
    
    [Authorize]
    public class TeamController : BaseController
    {
        public TeamController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<IActionResult> Index()
        {
            if (!HasProduct) return RedirectToAction("New", "Product");

            //var tasks = await TaskService.GetAll();
            var users = await UserService.GetAll();
            return View(new TeamViewModel 
            {
                //Tasks = tasks,
                Users = users
            });
        }

        //[HttpGet]
        //public async Task<IActionResult> AddUser(TeamViewModel model)
        //{
        //    if (!HasProduct) return RedirectToAction("New", "Product");

        //    var task = await TaskService.GetById(model.TaskId);
        //    if (task == null)
        //    {
        //        return BadRequest("Сначала нужно создать хотя бы одну задачу!");
        //    }
            
        //    var users = await UserService.GetAll();
        //    return View(new AddUserToTeamViewModel 
        //    {
        //        TaskId = task.ID,
        //        TaskName = task.Name,
        //        PosibleUsers = users.Where(x => !task.ResponsibleUser.Contains(x)).ToList()
        //    });
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddUser(AddUserToTeamViewModel model)
        //{
        //    var task = await TaskService.GetById(model.TaskId);

        //    foreach (var user in await UserService.GetByIds(model.UserIds))
        //    {
        //        if (!task.ResponsibleUser.Any(x => x.Id == user.Id))
        //        {
        //            task.ResponsibleUser.Add(user);
        //        }
        //    }
            
        //    await TaskService.Save(task);
            
        //    return RedirectToAction("Index", "Team");
        //}
    }
}
