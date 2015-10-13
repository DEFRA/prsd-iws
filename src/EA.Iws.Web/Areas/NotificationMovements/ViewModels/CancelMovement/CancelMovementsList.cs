namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.CancelMovement
{
    using System;

    [Serializable]
    public class CancelMovementsList
    {
        public int Number { get; set; }

        public Guid MovementId { get; set; }
    }
}