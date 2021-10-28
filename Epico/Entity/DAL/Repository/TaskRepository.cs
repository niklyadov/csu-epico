namespace Epico.Entity.DAL.Repository
{
    public class TaskRepository : BaseRepository<Task, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public TaskRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}