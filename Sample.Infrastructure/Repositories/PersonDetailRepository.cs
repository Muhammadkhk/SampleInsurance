using Microsoft.EntityFrameworkCore;
using Sample.Framework.Infrastructure;
using Sample.Domain.PersonDetail;
using Sample.Core.Infrastructure;

namespace Sample.Infrastructure.Repositories
{
    public class PersonDetailRepository : IPersonDetailRepository, IDisposable, IRepository
    {
        private readonly SampleDbContext _dbContext;

        public PersonDetailRepository(SampleDbContext dbContext) =>
            _dbContext = dbContext;


        public async Task Add(PersonDetail entity) =>
            await _dbContext.PersonDetails.AddAsync(entity);

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exists(PersonDetailId id)
        {
            var PersonDetail = await _dbContext.PersonDetails.FindAsync(id.Value);
            if (PersonDetail != null) 
                if(!PersonDetail.IsDeleted) return true;
            return false;
        }
    }
}
