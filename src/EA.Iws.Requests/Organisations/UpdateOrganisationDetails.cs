namespace EA.Iws.Requests.Organisations
{
    using System;
    using Core.Registration;
    using Prsd.Core.Mediator;

    public class UpdateOrganisationDetails : IRequest<Guid>
    {
        public UpdateOrganisationDetails(OrganisationRegistrationData organisation)
        {
            Organisation = organisation;
        }

        public OrganisationRegistrationData Organisation { get; set; }
    }
}
