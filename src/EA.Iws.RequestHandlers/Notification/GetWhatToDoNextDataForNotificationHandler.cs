namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetWhatToDoNextDataForNotificationHandler : IRequestHandler<GetWhatToDoNextDataForNotification, WhatToDoNextData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<NotificationApplication, UnitedKingdomCompetentAuthority, WhatToDoNextData> map;
        private readonly INotificationChargeCalculator chargeCalculator;

        public GetWhatToDoNextDataForNotificationHandler(IwsContext context, 
            IMapWithParameter<NotificationApplication, UnitedKingdomCompetentAuthority, WhatToDoNextData> map,
            INotificationChargeCalculator chargeCalculator)
        {
            this.context = context;
            this.map = map;
            this.chargeCalculator = chargeCalculator;
        }

        public async Task<WhatToDoNextData> HandleAsync(GetWhatToDoNextDataForNotification message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);

            var competentAuthority =
                await
                    context.UnitedKingdomCompetentAuthorities.SingleAsync(
                        n => n.Id == notification.CompetentAuthority.Value);

            var result = map.Map(notification, competentAuthority);

            result.Charge = await chargeCalculator.GetValue(message.Id);

            return result;
        }
    }
}
