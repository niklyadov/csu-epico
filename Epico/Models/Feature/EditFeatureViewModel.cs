using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Epico.Models
{
    public class EditFeatureViewModel
    {
        public int FeatureId { get; set; }

        [Required(ErrorMessage = "*Обязательное поле")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "*Обязательно")]
        [BindProperty]
        public int MetricId { get; set; }

        [Required(ErrorMessage = "*Обязательно")]
        [BindProperty]
        public List<int> UserIds { get; set; }

        [Required(ErrorMessage = "*Обязательно")]
        [BindProperty]
        public FeatureState State { get; set; }

        [Required(ErrorMessage = "*Обязательно")]
        [BindProperty]
        public RoadmapType Roadmap { get; set; }

        public List<User> PosibleUsers { get; set; }
        public List<Metric> PosibleMetrics { get; set; }
    }
}
