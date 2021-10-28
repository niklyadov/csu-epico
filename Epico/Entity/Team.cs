using System.Collections.Generic;

namespace Epico.Entity
{
    public class Team
    {
        public int ID { get; set; }
        public string Name  { get; set; }
        public TeamPosition Position  { get; set; }
        public List<User> Users { get; set; }
    }
}