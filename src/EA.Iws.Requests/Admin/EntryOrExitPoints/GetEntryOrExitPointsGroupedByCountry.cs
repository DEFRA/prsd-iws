namespace EA.Iws.Requests.Admin.EntryOrExitPoints
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.TransportRoute;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetEntryOrExitPointsGroupedByCountry : IRequest<EntryOrExitPointDataGroup[]>
    {
    }
}
