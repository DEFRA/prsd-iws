namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Submit
{
    using System;
    using Core.Notification;

    public class SubmitSideBarViewModel
    {
        public Guid NotificationId { get; set; }

        public string CompetentAuthorityName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string NotificationNumber { get; set; }

        public int Charge { get; set; }

        public Status Status { get; set; }
    }
}