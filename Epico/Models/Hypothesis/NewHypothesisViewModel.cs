using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class NewHypothesisViewModel
    {
        [Required(ErrorMessage = "*Обязательное поле")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "*Обязательно")]
        [BindProperty]
        public List<int> UserIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "*Обязательно")]
        [BindProperty]
        public int MetricId { get; set; }

        public List<User> PosibleUsers { get; set; }
        public List<Metric> PosibleMetrics { get; set; }
    }
}
