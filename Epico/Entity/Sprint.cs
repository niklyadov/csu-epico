using Epico.Entity.DAL;
using System;
using System.Collections.Generic;

namespace Epico.Entity
{
    public class Sprint : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Feature> Features { get; set; }
            = new List<Feature>();

    }
}