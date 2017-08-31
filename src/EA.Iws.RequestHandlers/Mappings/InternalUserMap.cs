namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Core.Admin;
    using Core.Authorization;
    using Domain;
    using Prsd.Core.Mapper;

    internal class InternalUserMap : IMap<InternalUser, InternalUserData>, IMapWithParameter<InternalUser, IEnumerable<Claim>, InternalUserData>
    {
        public InternalUserData Map(InternalUser source)
        {
            if (source == null)
            {
                return null;
            }

            return new InternalUserData
            {
                Id = source.Id,
                UserId = source.UserId,
                Email = source.User.Email,
                CompetentAuthority = source.CompetentAuthority,
                FirstName = source.User.FirstName,
                Surname = source.User.Surname,
                Status = source.Status,
                JobTitle = source.JobTitle,
                PhoneNumber = source.User.PhoneNumber,
                Role = UserRole.Internal
            };
        }

        public InternalUserData Map(InternalUser source, IEnumerable<Claim> parameter)
        {
            var user = Map(source);

            user.Role = GetRole(parameter);

            return user;
        }

        private static UserRole GetRole(IEnumerable<Claim> claims)
        {
            var claimsArray = claims as Claim[] ?? claims.ToArray();
            var isAdmin = claimsArray.Any(c => c.Type == ClaimTypes.Role && c.Value == UserRole.Administrator.ToString().ToLowerInvariant());
            var isReadOnly = claimsArray.Any(c => c.Type == ClaimTypes.Role && c.Value == UserRole.ReadOnly.ToString().ToLowerInvariant());

            if (isAdmin)
            {
                return UserRole.Administrator;
            }

            if (isReadOnly)
            {
                return UserRole.ReadOnly;
            }

            return UserRole.Internal;
        }
    }
}