using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewFeatureViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hypothesis { get; set; }
        [BindProperty]
        public List<int> Tasks { get; set; }
        [BindProperty]
        public List<int> Metrics { get; set; }

        public int ProjectId { get; set; }
        public List<Entity.Task> PosibleTasks { get; set; }
        public List<Metric> PosibleMetrics { get; set; }
    }
}
