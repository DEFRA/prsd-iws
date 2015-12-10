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

        public MovementSummaryTableRowViewModel(ImportMovementSummaryData movementSummaryData)
        {
            Number = movementSummaryData.Data.Number;
            if (movementSummaryData.Data.PreNotificationDate.HasValue)
            {
                PrenotificationDate = movementSummaryData.Data.PreNotificationDate.Value.DateTime;
            }
            ShipmentDate = movementSummaryData.Data.ActualDate.DateTime;
        }
    }
}