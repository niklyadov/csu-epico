using Epico.Entity;
using System;
using System.Collections.Generic;

namespace Epico.Models
{
    public class NewTaskViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int FeaturesId { get; set; } // List<Feature> несколько фич в одной задаче
        public DateTime DeadLine { get; set; }
        public List<Team> PosibleTeams { get; set; }
    }
}
