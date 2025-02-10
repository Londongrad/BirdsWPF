using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace BirdsCommonStandard
{
    public static class DbContextHelper
    {
        public static void Clear(this ChangeTracker changeTracker)
        {
            var undetachedEntriesCopy = changeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in undetachedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }

}
