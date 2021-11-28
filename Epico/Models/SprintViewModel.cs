using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class SprintViewModel
    {
        public int SprintId { get; set; }

        public int ProjectId { get; set; }
        public List<Sprint> Sprints { get; set; }
            = new List<Sprint>();
    }
}
