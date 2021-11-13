using Epico.Entity;
using Epico.Entity.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Services
{
    public class TeamService : ITeamService
    {
        private readonly TeamRepository _repository;
        public TeamService(TeamRepository repository)
        {
            _repository = repository;
        }
        public async Task<Team> AddTeam(string name, TeamPosition position, List<User> users)
        {
            return await _repository.Add(new Team
            {
                Name = name,
                Position = position,
                Users = users
            });
        }
    }
}
