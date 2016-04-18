namespace EA.Iws.Requests.Organisations
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Registration;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanEditOrganisationData)]
    public class UpdateOrganisationDetails : IRequest<Guid>
    {
        public UpdateOrganisationDetails(OrganisationRegistrationData organisation)
        {
            Organisation = organisation;
        }

        public OrganisationRegistrationData Organisation { get; set; }
    }
}
