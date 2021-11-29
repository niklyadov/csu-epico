using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class EditStateTaskViewModel
    {
        public int TaskId { get; set; }
        public Entity.Task Task { get; set; }
        [BindProperty] public TaskState State { get; set; }
    }
}
