namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class SetActualMovementCarriers : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public Dictionary<int, Guid> SelectedCarriers { get; private set; }

        public SetActualMovementCarriers(Guid movementId, Dictionary<int, Guid> selectedCarriers)
        {
            MovementId = movementId;
            SelectedCarriers = selectedCarriers;
        }
    }
}
