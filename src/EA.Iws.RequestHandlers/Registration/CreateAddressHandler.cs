namespace EA.Iws.RequestHandlers.Registration
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.Registration;

    internal class CreateAddressHandler : IRequestHandler<CreateAddress, Guid>
    {
        private readonly IwsContext db;

        public CreateAddressHandler(IwsContext db)
        {
            this.db = db;
        }

        public async Task<Guid> HandleAsync(CreateAddress command)
        {
            try
            {
                var country = await db.Countries.SingleAsync(c => c.Id == command.Address.CountryId);

                var address = new UserAddress(command.Address.StreetOrSuburb, command.Address.Address2, command.Address.TownOrCity, command.Address.Region,
                    command.Address.PostalCode, country.Name);

                db.Addresses.Add(address);

                await db.SaveChangesAsync();

                var user = await db.Users.SingleAsync(u => u.Id == command.UserId);

                user.Address = address;

                await db.SaveChangesAsync();

                return address.Id;
            }
            catch (Exception ex)
            {
                return new Guid();
            }
        }
    }
}