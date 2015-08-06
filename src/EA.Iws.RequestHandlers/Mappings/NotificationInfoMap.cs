namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class NotificationInfoMap : IMap<NotificationApplication, NotificationInfo>
    {
        private readonly IMap<NotificationApplication, OrganisationsInvolvedInfo> organisationsInvolvedInfoMap;
        private readonly IMap<NotificationApplication, RecoveryOperationInfo> recoveryOperationInfoMap;
        private readonly IMap<NotificationApplication, ClassifyYourWasteInfo> classifyYourWasteInfoMap;
        private readonly IMap<NotificationApplication, TransportationInfo> transportationInfoMap;
        private readonly IMap<NotificationApplication, JourneyInfo> journeyInfoMap;
        private readonly IMap<NotificationApplication, WasteRecoveryInfo> wasteRecoveryInfoMap;
        private readonly IMap<NotificationApplication, AmountsAndDatesInfo> amountsAndDatesInfoMap;

        public NotificationInfoMap(
            IMap<NotificationApplication, OrganisationsInvolvedInfo> organisationsInvolvedInfoMap,
            IMap<NotificationApplication, RecoveryOperationInfo> recoveryOperationInfoMap,
            IMap<NotificationApplication, ClassifyYourWasteInfo> classifyYourWasteInfoMap,
            IMap<NotificationApplication, TransportationInfo> transportationInfoMap,
            IMap<NotificationApplication, JourneyInfo> journeyInfoMap,
            IMap<NotificationApplication, WasteRecoveryInfo> wasteRecoveryInfoMap,
            IMap<NotificationApplication, AmountsAndDatesInfo> amountsAndDatesInfoMap)
        {
            this.organisationsInvolvedInfoMap = organisationsInvolvedInfoMap;
            this.recoveryOperationInfoMap = recoveryOperationInfoMap;
            this.classifyYourWasteInfoMap = classifyYourWasteInfoMap;
            this.transportationInfoMap = transportationInfoMap;
            this.journeyInfoMap = journeyInfoMap;
            this.wasteRecoveryInfoMap = wasteRecoveryInfoMap;
            this.amountsAndDatesInfoMap = amountsAndDatesInfoMap;
        }

        public NotificationInfo Map(NotificationApplication notification)
        {
            return new NotificationInfo
            {
                NotificationId = notification.Id,
                CompetentAuthority = (CompetentAuthority)notification.CompetentAuthority.Value,
                NotificationNumber = notification.NotificationNumber,
                OrganisationsInvolvedInfo = organisationsInvolvedInfoMap.Map(notification),
                RecoveryOperationInfo = recoveryOperationInfoMap.Map(notification),
                TransportationInfo = transportationInfoMap.Map(notification),
                JourneyInfo = journeyInfoMap.Map(notification),
                AmountsAndDatesInfo = amountsAndDatesInfoMap.Map(notification),
                ClassifyYourWasteInfo = classifyYourWasteInfoMap.Map(notification),
                WasteRecoveryInfo = wasteRecoveryInfoMap.Map(notification)
            };
        }
    }
}
