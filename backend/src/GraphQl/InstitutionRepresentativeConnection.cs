using Models = Icon.Models;
using System.Linq;
using GreenDonut;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using IPageInfo = HotChocolate.Types.Relay.IPageInfo;

namespace Icon.GraphQl
{
    public sealed class InstitutionRepresentativeConnection
      : Connection
    {
        public InstitutionRepresentativeConnection(
            Institution institution
            )
          : base(
              fromId: institution.Id,
              pageInfo: null!,
              requestTimestamp: institution.RequestTimestamp
              )
        {
        }

        public async Task<IReadOnlyList<InstitutionRepresentativeEdge>> GetEdges(
            [DataLoader] RepresentativesOfInstitutionIdentifiedByTimestampedIdAssociationDataLoader representativesLoader
            )
        {
            return (await representativesLoader.LoadAsync(
                  TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                  )
                )
              .Select(a => new InstitutionRepresentativeEdge(a))
              .ToList().AsReadOnly();
        }

        public sealed class RepresentativesOfInstitutionIdentifiedByTimestampedIdAssociationDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<InstitutionRepresentative, Models.Institution, Models.InstitutionRepresentative>
        {
            public RepresentativesOfInstitutionIdentifiedByTimestampedIdAssociationDataLoader(IQueryBus queryBus)
              : base(InstitutionRepresentative.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<User>> GetNodes(
            [DataLoader] RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader representativesLoader
            )
        {
            return representativesLoader.LoadAsync(
                TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                );
        }

        public sealed class RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<User, Models.Institution, Models.User>
        {
            public RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(User.FromModel, queryBus)
            {
            }
        }
    }
}