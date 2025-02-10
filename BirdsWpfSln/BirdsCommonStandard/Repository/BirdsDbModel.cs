using Microsoft.EntityFrameworkCore;
using System;

namespace BirdsCommonStandard
{
    public class BirdsDbModel : IBirdsModel
    {
        public IRepository<Bird> Birds { get; }
        public IRepository<Specie> Species { get; }

        protected readonly Func<DbContext> contextCreater;

        public BirdsDbModel(Func<DbContext> contextCreater)
        {
            this.contextCreater = contextCreater;
            Birds = new Repository<Bird>(contextCreater);
            Species = new Repository<Specie>(contextCreater);
        }
    }
}
