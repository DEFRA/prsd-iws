namespace EA.Iws.Requests.Shared
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadCountryData)]
    public class GetUnitedKingdomCompetentAuthorityInformation : IRequest<UnitedKingdomCompetentAuthorityData>
    {
        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public GetUnitedKingdomCompetentAuthorityInformation(UKCompetentAuthority competentAuthority)
        {
            CompetentAuthority = competentAuthority;
        }
    }
}
