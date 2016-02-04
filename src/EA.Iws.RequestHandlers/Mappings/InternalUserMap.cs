namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Admin;
    using Domain;
    using Prsd.Core.Mapper;

    internal class InternalUserMap : IMap<InternalUser, InternalUserData>
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
                PhoneNumber = source.User.PhoneNumber
            };
        }
    }
}