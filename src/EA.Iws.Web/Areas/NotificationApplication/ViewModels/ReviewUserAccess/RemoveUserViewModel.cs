namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.ReviewUserAccess
{
    using System;
    public class RemoveUserViewModel
    {
        public Guid NotificationId { get; set; }

        public Guid SharedUserId { get; set; }

        public string UserId { get; set; }
        public string EmailId { get; set; }
    }
}