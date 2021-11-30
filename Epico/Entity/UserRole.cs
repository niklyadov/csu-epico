using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Epico.Entity
{
    public class UserRole : IdentityRole<int>
    {
        // todo тут поля юзера можно добавить
        public string Position { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}