using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewTaskViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public int TeamId { get; set; } // List<Feature> несколько фич в одной задаче
        public DateTime DeadLine { get; set; }
        [BindProperty]
        public List<string> Users { get; set; }
        public List<User> PosibleUsers { get; set; }
    }
}
