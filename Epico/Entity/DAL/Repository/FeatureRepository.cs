namespace Epico.Entity.DAL.Repository
{
    public class FeatureRepository : BaseRepository<Feature, ApplicationContext>
    {
        private ApplicationContext _dbContext;
        public FeatureRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}