namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.NumberOfShipments
{
    using EA.Iws.Core.ImportNotification;
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Shared;
    using System;

    public class ConfirmViewModel
    {
        public int NewNumberOfShipments { get; set; }

        public int OldNumberOfShipments { get; set; }

        public Guid NotificationId { get; set; }

        public decimal CurrentCharge { get; set; }

        public decimal NewCharge { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public bool ShowAdditionalCharge { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }

        public ConfirmViewModel()
        {
        }

        public ConfirmViewModel(ConfirmNumberOfShipmentsChangeData data, UKCompetentAuthority competentAuthority, ImportNotificationStatus notificationStatus)
        {
            NotificationId = data.NotificationId;
            CurrentCharge = data.CurrentCharge;
            OldNumberOfShipments = data.CurrentNumberOfShipments;
            NewCharge = data.NewCharge;
            AdditionalCharge = new AdditionalChargeData()
            {
                NotificationId = NotificationId
            };
            CompetentAuthority = competentAuthority;
            NotificationStatus = notificationStatus;
            ShowAdditionalCharge = (competentAuthority == UKCompetentAuthority.England || competentAuthority == UKCompetentAuthority.Scotland) 
                                    && (notificationStatus == ImportNotificationStatus.Consented);
        }

        public bool IsIncrease
        {
            get { return NewNumberOfShipments > OldNumberOfShipments; }
        }

        public decimal IncreaseInCharge
        {
            get { return NewCharge - CurrentCharge; }
        }
    }
}