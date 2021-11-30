using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Epico.Entity
{
    public class User : IdentityUser
    {
        // todo тут поля юзера можно добавить
        public string Position { get; set; }
        public List<Task> Tasks { get; set; }
    }
}