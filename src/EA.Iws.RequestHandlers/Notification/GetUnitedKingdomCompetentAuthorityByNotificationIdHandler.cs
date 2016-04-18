namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetUnitedKingdomCompetentAuthorityByNotificationIdHandler : 
        IRequestHandler<GetUnitedKingdomCompetentAuthorityByNotificationId, UnitedKingdomCompetentAuthorityData>
    {
        private readonly IwsContext context;
        private readonly IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData> competentAuthorityMap;

        public GetUnitedKingdomCompetentAuthorityByNotificationIdHandler(IwsContext context, 
            IMap<UnitedKingdomCompetentAuthority, UnitedKingdomCompetentAuthorityData> competentAuthorityMap)
        {
            this.context = context;
            this.competentAuthorityMap = competentAuthorityMap;
        }

        public async Task<UnitedKingdomCompetentAuthorityData> HandleAsync(GetUnitedKingdomCompetentAuthorityByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(na => na.Id == message.Id);

            var competentAuthority =
                await
                    context.UnitedKingdomCompetentAuthorities.SingleAsync(
                        c => c.Id == (int)notification.CompetentAuthority);

            return competentAuthorityMap.Map(competentAuthority);
        }
    }
}
