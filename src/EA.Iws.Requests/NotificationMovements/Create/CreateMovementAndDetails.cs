namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class CreateMovementAndDetails : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }
        public NewMovementDetails NewMovementDetails { get; private set; }
        public DateTime ActualMovementDate { get; private set; }

        public CreateMovementAndDetails(Guid notificationId, 
            DateTime actualMovementDate,
            NewMovementDetails newMovementDetails)
        {
            ActualMovementDate = actualMovementDate;
            NewMovementDetails = newMovementDetails;
            NotificationId = notificationId;
        }
    }
}