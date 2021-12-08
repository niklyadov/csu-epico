using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class NewTaskViewModel
    {
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "*Обязательное поле")]
        public DateTime DeadLine { get; set; } = DateTime.Now;

        [BindProperty]
        public int UserId { get; set; }

        public List<User> PossibleUsers { get; set; }
            = new List<User>();
    }
}
