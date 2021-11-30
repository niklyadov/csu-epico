using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class NewFeatureViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Hypothesis { get; set; }
        [BindProperty] public RoadmapType Roadmap { get; set; }
        [BindProperty] public List<int> Tasks { get; set; } = new List<int>();
        [BindProperty] public List<int> Metrics { get; set; } = new List<int>();

        public int ProductId { get; set; }
        public List<Entity.Task> PosibleTasks { get; set; }
        public List<Metric> PosibleMetrics { get; set; }
    }
}
