using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class NewTaskByIdViewModel
    {
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*Обязательное поле")]
        public DateTime DeadLine { get; set; } = DateTime.Now;

        [BindProperty]
        public int UserId { get; set; }

        public List<User> PossibleUsers { get; set; }
            = new List<User>();

        //hidden
        public int FeatureId { get; set; }
    }
}
