namespace EA.Iws.Requests.Organisations
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Organisations;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadOrganisationData)]
    public class OrganisationById : IRequest<OrganisationData>
    {
        public readonly Guid Id;

        public OrganisationById(Guid id)
        {
            Id = id;
        }
    }
}