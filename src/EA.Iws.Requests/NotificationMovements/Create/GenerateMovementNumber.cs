namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Prsd.Core.Mediator;

    public class GenerateMovementNumber : IRequest<int>
    {
        public Guid NotificationId { get; private set; }

        public GenerateMovementNumber(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}