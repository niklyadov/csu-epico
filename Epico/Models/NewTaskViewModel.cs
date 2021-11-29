using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewTaskViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        [BindProperty]
        public List<string> Users { get; set; }

        public int ProductId { get; set; }
        public List<User> PosibleUsers { get; set; }
    }
}
 