namespace EA.Iws.Requests.Registration
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Registration;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanEditUserData)]
    public class CreateOrganisation : IRequest<Guid>
    {
        public CreateOrganisation(OrganisationRegistrationData organisation)
        {
            Organisation = organisation;
        }

        public OrganisationRegistrationData Organisation { get; set; }
    }
}