namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class CreateNotificationApplication : IRequest<Guid>
    {
        public WasteAction WasteAction { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }
    }
}