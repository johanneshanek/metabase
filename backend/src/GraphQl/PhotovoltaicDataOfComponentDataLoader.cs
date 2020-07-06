using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class PhotovoltaicDataOfComponentDataLoader
      : DataOfComponentDataLoader<Models.PhotovoltaicData, PhotovoltaicData>
    {
        public PhotovoltaicDataOfComponentDataLoader(IQueryBus queryBus)
          : base(PhotovoltaicData.FromModel, queryBus)
        {
        }
    }
}