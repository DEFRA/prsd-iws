namespace EA.Iws.Requests.Registration
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanEditUserData)]
    public class UpdateOrganisationOfUser : IRequest<bool>
    {
        public UpdateOrganisationOfUser(Guid organisationId)
        {
            OrganisationId = organisationId;
        }

        public Guid OrganisationId { get; private set; }
    }
}
