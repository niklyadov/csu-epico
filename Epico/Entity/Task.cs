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
        public List<Feature> Features { get; set; }
        public User Assignee { get; set; }
        public FeatureState State { get; set; } //todo: возможно надо добавить новое состояние
        public DateTime DeadLine { get; set; }
    }
}