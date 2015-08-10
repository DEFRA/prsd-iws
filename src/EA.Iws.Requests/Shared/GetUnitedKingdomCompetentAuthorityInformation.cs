namespace EA.Iws.Requests.Shared
{
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetUnitedKingdomCompetentAuthorityInformation : IRequest<UnitedKingdomCompetentAuthorityData>
    {
        public CompetentAuthority CompetentAuthority { get; private set; }

        public GetUnitedKingdomCompetentAuthorityInformation(CompetentAuthority competentAuthority)
        {
            CompetentAuthority = competentAuthority;
        }
    }
}
