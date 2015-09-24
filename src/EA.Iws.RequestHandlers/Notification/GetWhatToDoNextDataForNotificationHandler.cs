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
        private readonly NotificationChargeCalculator chargeCalculator;

        public GetWhatToDoNextDataForNotificationHandler(IwsContext context, 
            IMapWithParameter<NotificationApplication, UnitedKingdomCompetentAuthority, WhatToDoNextData> map,
            NotificationChargeCalculator chargeCalculator)
        {
            this.context = context;
            this.map = map;
            this.chargeCalculator = chargeCalculator;
        }

        public async Task<WhatToDoNextData> HandleAsync(GetWhatToDoNextDataForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.Id);
            var pricingStructures = await context.PricingStructures.ToArrayAsync();
            var shipmentInfo = await context.GetShipmentInfoAsync(message.Id);

            var competentAuthority =
                await
                    context.UnitedKingdomCompetentAuthorities.SingleAsync(
                        n => n.Id == notification.CompetentAuthority.Value);

            var result = map.Map(notification, competentAuthority);

            result.Charge = chargeCalculator.GetValue(pricingStructures, notification, shipmentInfo);

            return result;
        }
    }
}
