namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Core.Admin;
    using Domain;
    using Prsd.Core.Mapper;

    internal class InternalUserMap : IMap<User, InternalUser>
    {
        public InternalUser Map(User source)
        {
            if (source == null)
            {
                return null;
            }

            if (!source.IsInternal)
            {
                throw new InvalidOperationException("Cannot map an external user to an internal user! Id: " + source.Id);
            }

            if (!source.InternalUserStatus.HasValue)
            {
                throw new InvalidOperationException("Cannot map a user with no status to an internal user! Id: " + source.Id);
            }

            return new InternalUser
            {
                Id = source.Id,
                Email = source.Email,
                CompetentAuthority = source.CompetentAuthority,
                FirstName = source.FirstName,
                Surname = source.Surname,
                Status = source.InternalUserStatus.Value,
                JobTitle = source.JobTitle,
                PhoneNumber = source.PhoneNumber
            };
        }
    }
}
