using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewSprintViewModel
    {
        public string Name { get; set; }
        [BindProperty]
        public List<Feature> PosibleFeatures { get; set; }

        public int ProjectID { get; set; }
        public List<Feature> Features { get; set; } // List<Feature> 
    }
}
