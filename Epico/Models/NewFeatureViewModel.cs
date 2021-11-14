using Epico.Entity;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewFeatureViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hypothesis { get; set; }
        public int MetricId { get; set; } // List<Metric> // Надо будет сделать список
        public int TaskId { get; set; } // List<Task> // Надо будет сделать список

        public List<Entity.Task> PosibleTasks { get; set; }
        public List<Metric> PosibleMetrics { get; set; }
    }
}
