using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class EditFeatureViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Hypothesis { get; set; }
        [BindProperty]
        public List<int> Metrics { get; set; }
        [BindProperty]
        public List<int> Tasks { get; set; }
        public FeatureState State { get; set; }
        public RoadmapType Roadmap { get; set; }

        public int ProductId { get; set; }
        public List<Entity.Task> PosibleTasks { get; set; }
        public List<Metric> PosibleMetrics { get; set; }
        public IEnumerable<RoadmapType> RoadmapTypes { get; set; } = Enum.GetValues(typeof(RoadmapType)).Cast<RoadmapType>();
        public IEnumerable<FeatureState> StateTypes { get; set; } = Enum.GetValues(typeof(FeatureState)).Cast<FeatureState>();
    }
}
