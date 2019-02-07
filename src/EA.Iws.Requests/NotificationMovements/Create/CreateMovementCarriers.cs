namespace EA.Iws.Requests.NotificationMovements.Create
{
     using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class CreateMovementCarriers : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public Guid[] MovementId { get; private set; }

        public Dictionary<int, Guid> SelectedCarriers { get; private set; }

        public CreateMovementCarriers(Guid notificationId, Guid[] movementId, Dictionary<int, Guid> selectedCarriers)
        {
            NotificationId = notificationId;
            MovementId = movementId;
            SelectedCarriers = selectedCarriers;
        }
    }
}
