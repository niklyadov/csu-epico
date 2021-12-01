using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Epico.Entity
{
    public class User : IdentityUser<int>
    {
        // todo тут поля юзера можно добавить
        public string Position { get; set; }
        public virtual List<Task> Tasks { get; set; }
    }
}