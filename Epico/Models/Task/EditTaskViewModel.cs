﻿using Epico.Entity;
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
        [Required(ErrorMessage ="*Обязательное поле")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Description { get; set; }
        [Required(ErrorMessage = "*Обязательное поле")]
        public DateTime DeadLine { get; set; }
        public TaskState State { get; set; }
        [BindProperty]
        public List<string> Users { get; set; }

        public List<User> PosibleUsers { get; set; }
        public IEnumerable<TaskState> StateTypes { get; set; } = Enum.GetValues<TaskState>().Cast<TaskState>();
    }
}
