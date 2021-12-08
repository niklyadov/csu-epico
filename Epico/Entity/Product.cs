using Epico.Entity.DAL;
using System.Collections.Generic;

namespace Epico.Entity
{
    public class Product : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string ProductFormula { get; set; }
        public string OwnerUserId { get; set; }

        public List<Sprint> Sprints { get; set; }
            = new List<Sprint>();
    }
}