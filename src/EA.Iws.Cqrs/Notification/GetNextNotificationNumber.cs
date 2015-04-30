namespace EA.Iws.Cqrs.Notification
{
    using Core.Cqrs;
    using Domain;

    public class GetNextNotificationNumber : IQuery<int>
    {
        public GetNextNotificationNumber(UKCompetentAuthority competentAuthority)
        {
            CompetentAuthority = competentAuthority;
        }

        public UKCompetentAuthority CompetentAuthority { get; private set; }
    }
}