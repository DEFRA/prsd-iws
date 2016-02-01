namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Home
{
    using System;
    using Core.Notification;
    using Requests.Notification;

    public class ResubmissionSuccessViewModel
    {
        public ResubmissionSuccessViewModel()
        {
        }

        public ResubmissionSuccessViewModel(NotificationBasicInfo details)
        {
            NotificationId = details.NotificationId;
            NotificationNumber = details.NotificationNumber;
            CompetentAuthority = details.CompetentAuthority;
        }

        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public Guid NotificationId { get; set; }
    }
}