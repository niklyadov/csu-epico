using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class NewTaskViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DeadLine { get; set; }
        [BindProperty]
        public List<string> Users { get; set; }

        public int ProductId { get; set; }
        public List<User> PosibleUsers { get; set; }
    }
}
 