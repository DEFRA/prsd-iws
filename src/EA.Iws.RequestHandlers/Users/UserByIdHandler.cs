namespace EA.Iws.RequestHandlers.Users
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Organisations;
    using Core.Registration.Users;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Registration.Users;

    internal class UserByIdHandler : IRequestHandler<UserById, User>
    {
        private readonly IwsContext context;

        public UserByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<User> HandleAsync(UserById query)
        {
            return await context.Users.Select(u => new User
            {
                Email = u.Email,
                FirstName = u.FirstName,
                Id = u.Id,
                PhoneNumber = u.PhoneNumber,
                Surname = u.Surname,
                Organisation = new OrganisationData()
                {
                    Id = u.Organisation.Id,
                    Name = u.Organisation.Name,
                    Address = new AddressData()
                    {
                        Address2 = u.Organisation.Address.Address2,
                        CountryName = u.Organisation.Address.Country,
                        PostalCode = u.Organisation.Address.PostalCode,
                        StreetOrSuburb = u.Organisation.Address.Address1,
                        TownOrCity = u.Organisation.Address.TownOrCity
                    }
                }
            }).SingleOrDefaultAsync(u => u.Id == query.Id);
        }
    }
}