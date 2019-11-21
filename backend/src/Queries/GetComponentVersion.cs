using Validatable = Icon.Validatable;
using Guid = System.Guid;
using Icon.Infrastructure.Query;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public class GetComponentVersion
      : Validatable, IQuery<Result<Models.ComponentVersion, IError>>
    {
        public Guid ComponentVersionId { get; }
        public DateTime Timestamp { get; } // TODO ZonedDateTime

        public GetComponentVersion(
            Guid componentVersionId,
            DateTime timestamp
            )
        {
            ComponentVersionId = componentVersionId;
            Timestamp = timestamp;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              ComponentVersionId != Guid.Empty &&
              Timestamp != DateTime.MinValue;
        }
    }
}