using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewFeatureViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hypothesis { get; set; }
        public Team Team { get; set; }
        public Metric Metric { get; set; } // List<Metric>
                                           // Надо будет сделать список
        
        public List<Team> PosibleTeams { get; set; }
        public List<Metric> PosibleMetrics { get; set; }
    }
}
