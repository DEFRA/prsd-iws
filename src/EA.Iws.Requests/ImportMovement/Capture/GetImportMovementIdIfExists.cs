namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Prsd.Core.Mediator;

    public class GetImportMovementIdIfExists : IRequest<Guid?>
    {
        public Guid NotificationId { get; private set; }

        public int Number { get; private set; }

        public GetImportMovementIdIfExists(Guid notificationId, int number)
        {
            NotificationId = notificationId;
            Number = number;
        }
    }
}
