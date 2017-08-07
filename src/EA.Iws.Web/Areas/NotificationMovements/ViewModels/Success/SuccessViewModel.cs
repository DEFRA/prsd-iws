namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Success
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Movement;

    public class SuccessViewModel
    {
        public SuccessViewModel()
        {
        }

        public SuccessViewModel(IEnumerable<MovementInfo> movements)
        {
            Shipments = movements.Select(m => string.Format("Shipment {0}", m.ShipmentNumber));
        }

        public IEnumerable<string> Shipments { get; set; }
    }
}