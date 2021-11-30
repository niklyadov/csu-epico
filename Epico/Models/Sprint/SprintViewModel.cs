using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class SprintViewModel
    {
        public int SprintId { get; set; }

        public List<Sprint> Sprints { get; set; }
            = new List<Sprint>();
        public bool SprintError { get; set; }
    }
}
