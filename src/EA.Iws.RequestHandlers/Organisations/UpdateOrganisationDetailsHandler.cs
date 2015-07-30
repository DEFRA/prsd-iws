namespace EA.Iws.RequestHandlers.Organisations
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
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
            var country = await context.Countries.SingleAsync(c => c.Id == orgData.CountryId);

            var org = await context.Organisations.SingleAsync(x => x.Id == orgData.OrganisationId);
            var address = new Address(orgData.Building, orgData.Address1, orgData.Address2, orgData.TownOrCity,
                                        null, orgData.Postcode, country.Name);

            BusinessType type = BusinessType.FromBusinessType(orgData.BusinessType);
            
            org = new Organisation(orgData.Name, address, type, orgData.OtherDescription);
            context.Organisations.Add(org);
            await context.SaveChangesAsync();

            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());
            user.UpdateOrganisationOfUser(org);
            await context.SaveChangesAsync();

            return org.Id;
        }
    }
}