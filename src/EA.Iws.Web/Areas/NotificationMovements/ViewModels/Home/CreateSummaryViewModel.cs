namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Home
{
    using System.Collections.Generic;
    using Core.Movement;

    public class CreateSummaryViewModel
    {
        public BasicMovementSummary SummaryData { get; set; }

        public IList<int> NewShipmentNumbers { get; set; }
    }
}