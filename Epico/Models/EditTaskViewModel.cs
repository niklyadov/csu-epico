using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class EditTaskViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public TaskState State { get; set; }
        [BindProperty]
        public List<string> Users { get; set; }

        public int ProjectId { get; set; }
        public List<User> PosibleUsers { get; set; }
        public IEnumerable<TaskState> StateTypes { get; set; } = Enum.GetValues<TaskState>().Cast<TaskState>();
    }
}
