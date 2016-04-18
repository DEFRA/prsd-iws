namespace EA.Iws.Requests.Organisations
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Registration;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadOrganisationData)]
    public class GetOrganisationDetailsByUser : IRequest<OrganisationRegistrationData>
    {
    }
}
