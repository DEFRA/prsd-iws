namespace EA.Iws.RequestHandlers.Organisations
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Organisations;

    internal class UpdateOrganisationDetailsHandler : IRequestHandler<UpdateOrganisationDetails, Guid>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public UpdateOrganisationDetailsHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<Guid> HandleAsync(UpdateOrganisationDetails message)
        {
            var orgData = message.Organisation;

            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());
            await context.SaveChangesAsync();

            var country = await context.Countries.SingleAsync(c => c.Id == orgData.Address.CountryId);

            var address = user.Address;
            address.UpdateAddress(new UserAddress(new Address(orgData.Address.StreetOrSuburb, orgData.Address.Address2, orgData.Address.TownOrCity, orgData.Address.Region, orgData.Address.PostalCode, country.Name)));
            await context.SaveChangesAsync();

            return address.Id;
        }
    }
}