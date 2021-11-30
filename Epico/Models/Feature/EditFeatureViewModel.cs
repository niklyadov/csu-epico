using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class EditFeatureViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Hypothesis { get; set; }
        [BindProperty] public List<int> Metrics { get; set; }
        [BindProperty] public List<int> Tasks { get; set; }
        [BindProperty] public FeatureState State { get; set; }
        [BindProperty] public RoadmapType Roadmap { get; set; }

        public int ProductId { get; set; }
        public List<Entity.Task> PosibleTasks { get; set; }
        public List<Metric> PosibleMetrics { get; set; }
    }
}
