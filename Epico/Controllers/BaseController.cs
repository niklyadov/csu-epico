using System;
using Epico.Services;
using Microsoft.AspNetCore.Mvc;

namespace Epico.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AccountService AccountService;
        protected readonly FeatureService FeatureService;
        protected readonly MetricService MetricService;
        protected readonly ProjectService ProjectService;
        protected readonly SprintService SprintService;
        protected readonly TaskService TaskService;
        protected readonly UserService UserService;
        
        public BaseController(IServiceProvider serviceProvider)
        {
            AccountService = serviceProvider.GetService(typeof(AccountService)) as AccountService;
            FeatureService = serviceProvider.GetService(typeof(FeatureService)) as FeatureService;
            MetricService = serviceProvider.GetService(typeof(MetricService)) as MetricService;
            AccountService = serviceProvider.GetService(typeof(AccountService)) as AccountService;
            ProjectService = serviceProvider.GetService(typeof(ProjectService)) as ProjectService;
            SprintService = serviceProvider.GetService(typeof(SprintService)) as SprintService;
            TaskService = serviceProvider.GetService(typeof(TaskService)) as TaskService;
            UserService = serviceProvider.GetService(typeof(UserService)) as UserService;
        }
    }
}