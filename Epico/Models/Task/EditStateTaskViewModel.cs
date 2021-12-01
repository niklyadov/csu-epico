using Epico.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Epico.Models
{
    public class EditStateTaskViewModel
    {
        public int TaskId { get; set; }
        public Entity.Task Task { get; set; }
        [BindProperty] public TaskState State { get; set; }
    }
}
