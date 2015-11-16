namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Home
{
    using Core.Movement;

    public class CreateSummaryViewModel
    {
        public BasicMovementSummary SummaryData { get; set; }

        public int NewShipmentNumber { get; set; }
    }
}