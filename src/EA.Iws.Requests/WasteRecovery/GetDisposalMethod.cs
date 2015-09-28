namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Prsd.Core.Mediator;

    public class GetDisposalMethod : IRequest<string>
    {
        public Guid NotificationId { get; private set; }

        public GetDisposalMethod(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
