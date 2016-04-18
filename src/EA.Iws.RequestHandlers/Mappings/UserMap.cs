namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Admin;
    using Domain;
    using Prsd.Core.Mapper;

    internal class UserMap : IMap<User, ChangeUserData>
    {
        public ChangeUserData Map(User source)
        {
            var user = new ChangeUserData
            {
                UserId = source.Id,
                FirstName = source.FirstName,
                LastName = source.Surname,
                PhoneNumber = source.PhoneNumber,
                Email = source.Email,
                OrganisationName = source.Organisation != null ? source.Organisation.Name : string.Empty
            };

            return user;
        }
    }
}
