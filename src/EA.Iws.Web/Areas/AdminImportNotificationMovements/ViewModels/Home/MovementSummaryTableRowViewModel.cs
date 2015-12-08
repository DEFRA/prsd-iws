namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Home
{
    using System;
    using Core.ImportMovement;
    using Core.Shared;

    public class MovementSummaryTableRowViewModel
    {
        public int Number { get; set; }
        
        public DateTime? PrenotificationDate { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public ShipmentQuantityUnits? Unit { get; set; }

        public MovementSummaryTableRowViewModel(ImportMovement movement)
        {
            Number = movement.Dates.Number;
            if (movement.Dates.PreNotificationDate.HasValue)
            {
                PrenotificationDate = movement.Dates.PreNotificationDate.Value.DateTime;
            }
            ShipmentDate = movement.Dates.ActualDate.DateTime;
        }
    }
}