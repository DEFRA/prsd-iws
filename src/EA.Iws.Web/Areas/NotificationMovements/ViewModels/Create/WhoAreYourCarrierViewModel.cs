namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System;
    using System.Collections.Generic;

    public class WhoAreYourCarrierViewModel
    {
        public bool AddCarriersLater { get; set; }
        public IEnumerable<Guid> MovementIds { get; set; }
    }
}