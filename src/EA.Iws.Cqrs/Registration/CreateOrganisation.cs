namespace EA.Iws.Cqrs.Registration
{
    using Api.Client.Entities;
    using Domain;
    using Iws.Core.Cqrs;

    public class CreateOrganisation : ICommand
    {
        public CreateOrganisation(OrganisationRegistrationData organisation)
        {
            Organisation = organisation;
        }

        public OrganisationRegistrationData Organisation { get; set; }
    }
}