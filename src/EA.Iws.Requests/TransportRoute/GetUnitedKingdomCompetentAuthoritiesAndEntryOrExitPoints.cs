namespace EA.Iws.Requests.TransportRoute
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetUnitedKingdomCompetentAuthoritiesAndEntryOrExitPoints : IRequest<CompetentAuthorityAndEntryOrExitPointData>
    {
    }
}
