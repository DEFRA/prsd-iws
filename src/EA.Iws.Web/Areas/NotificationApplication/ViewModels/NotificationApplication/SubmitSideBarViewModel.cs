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

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public int Charge { get; set; }

        public NotificationStatus Status { get; set; }

        public bool IsNotificationComplete { get; set; }

        public bool IsOwner { get; set; }

        public bool IsSharedUser { get; set; }

        public bool IsInternalUser { get; set; }

        public string AccessLevelText
        {
            get
            {
                return this.IsOwner == true ? "Owner" : this.IsSharedUser == true ? "Administrator" : string.Empty;
            }
        }

        public bool ShowSubmitButton
        {
            get
            {
                return IsNotificationComplete && Status == NotificationStatus.NotSubmitted;
            }
        }

        public bool ShowDisabledSubmitButtonAndGuidanceText
        {
            get
            {
                return !IsNotificationComplete && Status == NotificationStatus.NotSubmitted;
            }
        }

        public bool ShowResubmitButton
        {
            get
            {
                return Status == NotificationStatus.Unlocked;
            }
        }

        public bool ShowViewUpdateHistoryLink
        {
            get
            {
                return !IsInternalUser && (IsOwner || IsSharedUser);
            }
        }

        public SubmitSideBarViewModel()
        {
        }

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