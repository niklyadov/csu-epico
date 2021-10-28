using System.Collections.Generic;

namespace Epico.Entity
{
    public class Roadmap
    {
        public int ID { get; set; }
        public Milestone Milestone { get; set; }
        public List<Feature> Features { get; set; }
    }
}