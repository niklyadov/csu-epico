using System.Collections.Generic;
using Epico.Entity.DAL;

namespace Epico.Entity
{
    public class Team : IEntity
    {
        public int ID { get; set; }
        public string Name  { get; set; }
        public TeamPosition Position  { get; set; }
        public List<User> Users { get; set; }
    }
}