using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Queries;

namespace Icon.Queries
{
    public sealed class GetModelsForTimestampedIds<M>
      : IQuery<IEnumerable<Result<M, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        private GetModelsForTimestampedIds(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<GetModelsForTimestampedIds<M>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetModelsForTimestampedIds<M>, Errors>(
                    new GetModelsForTimestampedIds<M>(
                        timestampedIds
                        )
                    );
        }
    }
}