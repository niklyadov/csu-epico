using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewSprintViewModel
    {
        public string Name { get; set; }
        [BindProperty]
        public List<int> Features { get; set; }

        public int ProjectId { get; set; }
        public List<Feature> PosibleFeatures { get; set; }
    }
}
