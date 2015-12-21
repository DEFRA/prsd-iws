namespace EA.Iws.RequestHandlers.Notification
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetWhatToDoNextDataForNotificationHandler : IRequestHandler<GetWhatToDoNextDataForNotification, WhatToDoNextData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<NotificationApplication, UnitedKingdomCompetentAuthority, WhatToDoNextData> map;
        private readonly INotificationChargeCalculator chargeCalculator;
        private readonly INotificationTransactionCalculator transactionCalculator;

        public GetWhatToDoNextDataForNotificationHandler(IwsContext context, 
            IMapWithParameter<NotificationApplication, UnitedKingdomCompetentAuthority, WhatToDoNextData> map,
            INotificationChargeCalculator chargeCalculator,
            INotificationTransactionCalculator transactionCalculator)
        {
            this.context = context;
            this.map = map;
            this.chargeCalculator = chargeCalculator;
            this.transactionCalculator = transactionCalculator;
        }

        public async Task<WhatToDoNextData> HandleAsync(GetWhatToDoNextDataForNotification message)
        {
            var notification = await context.GetNotificationApplication(message.Id);

            var competentAuthority = await context.UnitedKingdomCompetentAuthorities.SingleAsync(
                        n => n.Id == notification.CompetentAuthority.Value);

            var result = map.Map(notification, competentAuthority);

            result.Charge = await chargeCalculator.GetValue(message.Id);

            result.AmountPaid = await transactionCalculator.TotalPaid(message.Id);

            return result;
        }
    }
}
