namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.NumberOfShipments
{
    using System;
    using Core.NotificationAssessment;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Shared;

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

        public NotificationStatus NotificationStatus { get; set; }

        public ConfirmViewModel()
        {
        }

        public ConfirmViewModel(ConfirmNumberOfShipmentsChangeData data, UKCompetentAuthority competentAuthority, NotificationStatus notificationStatus)
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
            ShowAdditionalCharge = ((competentAuthority == UKCompetentAuthority.England ||
                                competentAuthority == UKCompetentAuthority.Scotland) &&
                                ((notificationStatus == NotificationStatus.Consented) ||
                                (notificationStatus == NotificationStatus.ConsentedUnlock) ||
                                (notificationStatus == NotificationStatus.Transmitted) ||
                                (notificationStatus == NotificationStatus.DecisionRequiredBy) ||
                                (notificationStatus == NotificationStatus.Reassessment))) ? true : false;
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