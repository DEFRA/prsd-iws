namespace EA.Iws.Core.Movement.Bulk
{
    using System.Collections.Generic;

    public class PrenotificationMovementCollection
    {
        public List<PrenotificationMovement> PrenotificationMovements { get; private set; }

        public PrenotificationMovementCollection()
        {
            PrenotificationMovements = new List<PrenotificationMovement>();
        }

        public void Add(PrenotificationMovement prenotificationMovement)
        {
            PrenotificationMovements.Add(prenotificationMovement);
        }
    }
}
