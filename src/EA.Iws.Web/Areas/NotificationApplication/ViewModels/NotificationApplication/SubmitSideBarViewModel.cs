namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.Notification;
    using Core.NotificationAssessment;
    using EA.Prsd.Core.Helpers;

    public class SubmitSideBarViewModel
    {
        public Guid NotificationId { get; set; }

        public string CompetentAuthorityName { get; set; }

        public DateTime CreatedDate { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public int Charge { get; set; }

        public NotificationStatus Status { get; set; }

        public bool IsNotificationComplete { get; set; }

        public SubmitSideBarViewModel(SubmitSummaryData submitSummaryData, int notificationCharge, NotificationApplicationCompletionProgress progress)
        {
            NotificationId = submitSummaryData.NotificationId;
            CompetentAuthorityName = EnumHelper.GetDisplayName(submitSummaryData.CompetentAuthority);
            CreatedDate = submitSummaryData.CreatedDate;
            NotificationNumber = submitSummaryData.NotificationNumber;
            Charge = notificationCharge;
            Status = submitSummaryData.Status;
            IsNotificationComplete = progress.IsAllComplete;
            CompetentAuthority = submitSummaryData.CompetentAuthority;
        }
    }
}