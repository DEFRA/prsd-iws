namespace EA.Iws.Requests.Registration
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanEditUserData)]
    public class LinkUserToOrganisation : IRequest<bool>
    {
        private readonly Guid organisationId;

        public LinkUserToOrganisation(Guid organisationId)
        {
            this.organisationId = organisationId;
        }

        public Guid OrganisationId
        {
            get { return organisationId; }
        }
    }
}