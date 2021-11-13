using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Services
{
    public interface ITeamService
    {
        public Task<Team> AddTeam(string name, TeamPosition position, List<User> users);
    }
}
