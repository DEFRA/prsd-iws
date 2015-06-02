namespace EA.Iws.Web.ViewModels.Carrier
{
    using System;
    using System.Collections.Generic;
    using Requests.Carriers;

    public class CarrierListViewModel
    {
        public Guid NotificationId { get; set; }

        public IList<CarrierData> Carriers { get; set; }
    }
}