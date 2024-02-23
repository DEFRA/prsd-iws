namespace EA.Iws.RequestHandlers.Notification
{
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Requests.Notification;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal class GetNotificationAdditionalChargesHandler : IRequestHandler<GetNotificationAdditionalCharges, IList<NotificationAdditionalChargeForDisplay>>
    {
        private readonly IMapper mapper;
        private readonly INotificationAdditionalChargeRepository repository;

        public GetNotificationAdditionalChargesHandler(IMapper mapper, INotificationAdditionalChargeRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<IList<NotificationAdditionalChargeForDisplay>> HandleAsync(GetNotificationAdditionalCharges message)
        {
            IEnumerable<AdditionalCharge> notificationAdditionalCharges = await repository.GetPagedNotificationAdditionalChargesById(message.NotificationId);

            var notificationAdditionChargeData = new List<NotificationAdditionalChargeForDisplay>();
            if (notificationAdditionalCharges != null && notificationAdditionalCharges.Count() > 0)
            {
                foreach (var additionalCharge in notificationAdditionalCharges)
                {
                    var charge = new NotificationAdditionalChargeForDisplay()
                    {
                        ChangeDetailType = additionalCharge.ChangeDetailType,
                        ChargeAmount = additionalCharge.ChargeAmount,
                        ChargeDate = additionalCharge.ChargeDate,
                        Comments = additionalCharge.Comments
                    };
                    notificationAdditionChargeData.Add(charge);
                }
            }

            return notificationAdditionChargeData;
        }
    }
}
