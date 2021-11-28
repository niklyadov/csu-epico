using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class AddSprintViewModel
    {
        public int FeatureId { get; set; }
        public int SprintId { get; set; }
        public string SprintName { get; set; }
        public List<Feature> Features { get; set; }
    }
}
