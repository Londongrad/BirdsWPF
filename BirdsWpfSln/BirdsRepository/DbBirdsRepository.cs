using BirdsCommon.Repository;
using Microsoft.EntityFrameworkCore;

namespace BirdsRepository
{
    public class DbBirdsRepository : Repository<Bird>
    {
        public DbBirdsRepository(Func<DbContext> contextCreator)
            : base(contextCreator)
        { }

        public DbBirdsRepository(string dbSQLiteFullName)
            : this(() => new ApplicationDbContext(dbSQLiteFullName))
        { }
    }
}
