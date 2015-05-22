namespace EA.Iws.Requests.Carriers
{
    using System;
    using Prsd.Core.Mediator;

    public class AddCarrierToNotification : IRequest<Guid>
    {
        public CarrierData CarrierData { get; private set; }

        public AddCarrierToNotification(CarrierData carrierData)
        {
            this.CarrierData = carrierData;
        }
    }
}
