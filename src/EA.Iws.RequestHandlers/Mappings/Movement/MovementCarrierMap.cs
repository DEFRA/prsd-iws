namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Carriers;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    public class MovementCarrierMap : IMap<Movement, Dictionary<int, CarrierData>>
    {
        private readonly IMap<Carrier, CarrierData> carrierMap;

        public MovementCarrierMap(IMap<Carrier, CarrierData> carrierMap)
        {
            this.carrierMap = carrierMap;
        }

        public Dictionary<int, CarrierData> Map(Movement source)
        {
            if (source == null)
            {
                return null;
            }
            else
            {
                return source.MovementCarriers
                    .ToDictionary(mc => mc.Order, mc => carrierMap.Map(mc.Carrier));
            }
        }
    }
}
