using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization
{
    public static class CommonAuthorization
    {
        public static async Task<bool> IsOwner(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return await FetchRole(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            ).ConfigureAwait(false)
            == Enumerations.InstitutionRepresentativeRole.OWNER;
        }

        public static async Task<bool> IsAtLeastMaintainer(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            var role =
                await FetchRole(
                claimsPrincipal,
                institutionId,
                userManager,
                context,
                cancellationToken
            ).ConfigureAwait(false);
            return
                role == Enumerations.InstitutionRepresentativeRole.OWNER
                || role == Enumerations.InstitutionRepresentativeRole.MAINTAINER;
        }

        private static async Task<Enumerations.InstitutionRepresentativeRole?> FetchRole(
            ClaimsPrincipal claimsPrincipal,
            Guid institutionId,
            UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return null;
            }
            var wrappedRole =
                await context.InstitutionRepresentatives.AsQueryable()
                .Where(x =>
                    x.InstitutionId == institutionId &&
                    x.UserId == user.Id
                    )
                .Select(x => new { x.Role }) // We wrap the role in an object whose default value is `null`. Note that enumerations have the first value as default value.
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            return wrappedRole?.Role;
        }
    }
}