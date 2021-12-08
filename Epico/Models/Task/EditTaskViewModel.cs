using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Epico.Models
{
    public class EditTaskViewModel
    {
        public int TaskId { get; set; }
        public int FeatureId { get; set; }

        [Required(ErrorMessage ="*Обязательное поле")]
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public int State { get; set; }
        [BindProperty]
        public int UserId { get; set; }

        public List<User> PosibleUsers { get; set; }
    }
}
