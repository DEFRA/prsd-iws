namespace EA.Iws.Cqrs.Registration
{
    using Api.Client.Entities;
    using Domain;
    using Iws.Core.Cqrs;

    public class CreateOrganisation : ICommand
    {
        public CreateOrganisation(OrganisationRegistrationData organisation)
        {
            var address = new Address(organisation.Address1, organisation.TownOrCity, organisation.Postcode, new Country());
            Organisation = new Organisation(organisation.Name, address, organisation.EntityType);
        }

        public Organisation Organisation { get; set; }
    }
}