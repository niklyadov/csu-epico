using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class AddRoadmapViewModel
    {
        public RoadmapType Roadmap { get; set; }
        public List<Feature> Features { get; set; }
        
        public int FeatureId { get; set; }
    }
}
