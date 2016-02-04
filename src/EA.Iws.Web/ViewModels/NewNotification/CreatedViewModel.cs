namespace EA.Iws.Web.ViewModels.NewNotification
{
    using System;
    using Core.Notification;

    public class CreatedViewModel
    {
        public string NotificationNumber { get; set; }

        public Guid NotificationId { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }
    }
}