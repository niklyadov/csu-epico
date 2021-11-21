using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Epico.Models;
using Microsoft.AspNetCore.Authorization;

namespace Epico.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}