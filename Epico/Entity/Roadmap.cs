using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Epico.Entity.DAL;

namespace Epico.Entity
{
    public class Roadmap : IEntity
    {
        public int ID { get; set; }
        [NotMapped]
        public Milestone Milestone { get; set; }
        public List<Feature> Features { get; set; }
    }
}