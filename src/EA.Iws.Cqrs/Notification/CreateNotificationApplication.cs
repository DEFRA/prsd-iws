namespace EA.Iws.Cqrs.Notification
{
    using System;
    using Core.Cqrs;
    using Domain;
    using Domain.Notification;

    public class CreateNotificationApplication : ICommand
    {
        public WasteAction WasteAction { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public Guid NotificationId { get; set; }
    }
}