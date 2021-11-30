using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class AddRoadmapViewModel
    {
        public RoadmapType Roadmap { get; set; }
        public List<Feature> Features { get; set; }
        
        public int FeatureId { get; set; }
    }
}
