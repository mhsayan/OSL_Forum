using System.Data.Entity;

namespace OSL.Forum.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;

        protected UnitOfWork()
        {

        }

        protected void Resolve(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose() => _dbContext?.Dispose();
        public void Save() => _dbContext?.SaveChanges();
    }
}
