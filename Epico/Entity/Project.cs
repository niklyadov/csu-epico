using System.Collections.Generic;

namespace Epico.Entity
{
    public class Project
    {
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string ProductFormula { get; set; }
        public Team Team { get; set; }
        public List<Metric> Metrics { get; set; }
        public List<Sprint> Sprints { get; set; }
        public List<Roadmap> Roadmaps { get; set; }
    }
}