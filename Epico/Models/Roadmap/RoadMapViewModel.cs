using Epico.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Models
{
    public class RoadMapViewModel
    {
        public List<Feature> Features { get; set; }
        public int Roadmap { get; set; }
    }
}
