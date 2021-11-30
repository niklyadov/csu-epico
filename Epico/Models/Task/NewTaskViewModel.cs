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
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Description { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public DateTime DeadLine { get; set; }
        [BindProperty]
        public List<string> Users { get; set; }

        public List<User> PossibleUsers { get; set; }
            = new List<User>();
    }
}
 