namespace EA.Iws.Requests.Registration
{
    using System;
    using Prsd.Core.Mediator;

    public class UpdateOrganisationOfUser : IRequest<bool>
    {
        public UpdateOrganisationOfUser(Guid organisationId)
        {
            OrganisationId = organisationId;
        }

        public Guid OrganisationId { get; private set; }
    }
}
