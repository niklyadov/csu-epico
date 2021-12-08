using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Epico.Entity
{
    public class User : IdentityUser<int>
    {
        // todo тут поля юзера можно добавить
        public string Position { get; set; }
        public virtual List<Task> Tasks { get; set; }
        public virtual List<Feature> Features { get; set; }
    }
}