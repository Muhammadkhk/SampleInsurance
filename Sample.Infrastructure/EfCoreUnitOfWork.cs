using Sample.Core.Infrastructure;
using Sample.Framework.Infrastructure;

namespace Sample.Infrastructure
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly SampleDbContext _dbContext;

        public EfCoreUnitOfWork(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task Commit() => _dbContext.SaveChangesAsync();
    }
}