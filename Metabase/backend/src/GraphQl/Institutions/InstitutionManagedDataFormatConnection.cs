using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManagedDataFormatConnection
        : Connection<Data.Institution, Data.DataFormat, InstitutionManagedDataFormatsByInstitutionIdDataLoader, InstitutionManagedDataFormatEdge>
    {
        public InstitutionManagedDataFormatConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionManagedDataFormatEdge(x)
                )
        {
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public Task<bool> CanCurrentUserAddEdgeAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return DataFormatAuthorization.IsAuthorizedToCreateDataFormatForInstitution(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}