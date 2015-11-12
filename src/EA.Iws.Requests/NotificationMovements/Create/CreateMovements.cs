namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Prsd.Core.Mediator;

    public class CreateMovements : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }
        public NewMovements NewMovements { get; private set; }

        public CreateMovements(Guid notificationId, NewMovements newMovements)
        {
            NewMovements = newMovements;
            NotificationId = notificationId;
        }
    }
}