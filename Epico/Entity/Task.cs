using System;
using System.Collections.Generic;
using Epico.Entity.DAL;

namespace Epico.Entity
{
    public class Task : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskState State { get; set; }
        public DateTime DeadLine { get; set; }

        public List<Feature> Features { get; set; }
            = new List<Feature>();

        public int? ResponsibleUserId { get; set; }
        public User? ResponsibleUser { get; set; }
        public bool HasResponsibleUser => ResponsibleUserId != null;
    }
}