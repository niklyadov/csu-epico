using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class AddFeatureViewModel
    {
        public string SprintName { get; set; }
        public int SprintId { get; set; }
        public List<Feature> PossibleFeatures { get; set; }
        public int FeatureId { get; set; }
    }
}
