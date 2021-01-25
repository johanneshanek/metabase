using Microsoft.EntityFrameworkCore;
using GreenDonut;
using Metabase.GraphQl.Entitys;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentByIdDataLoader
      : EntityByIdDataLoader<Data.Component>
    {
        public ComponentByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(
                batchScheduler,
                dbContextFactory,
                dbContext => dbContext.Components
                )
        {
        }
    }
}
