using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

namespace Metabase.Data
{
    public sealed class Role : IdentityRole<Guid>
    {
        public const string Administrator = "Administrator";

        public ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
    }
}