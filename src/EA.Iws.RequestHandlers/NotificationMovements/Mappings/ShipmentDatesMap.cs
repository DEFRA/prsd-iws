namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using Core.Movement;
    using Domain;
    using Prsd.Core.Mapper;

    internal class ShipmentDatesMap : IMap<DateRange, ShipmentDates>
    {
        public ShipmentDates Map(DateRange source)
        {
            return new ShipmentDates
            {
                StartDate = source.From,
                EndDate = source.To
            };
        }
    }
}