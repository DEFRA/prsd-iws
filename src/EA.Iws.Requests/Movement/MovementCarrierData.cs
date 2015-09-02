namespace EA.Iws.Requests.Movement
{
    using System.Collections.Generic;
    using Core.Carriers;

    public class MovementCarrierData
    {
        public IList<CarrierData> NotificationCarriers { get; set; }

        public Dictionary<int, CarrierData> SelectedCarriers { get; set; }
    }
}
