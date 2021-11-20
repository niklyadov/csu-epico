using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewSprintViewModel
    {
        public string Name { get; set; }
        public int FeatureId { get; set; } // List<Feature> 
        public int ProjectID { get; set; }

        public List<Feature> PosibleFeatures { get; set; } // List<Feature> 
    }
}
