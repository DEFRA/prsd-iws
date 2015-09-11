namespace EA.Iws.Web.Mappings.Movement
{
    using System;
    using System.Linq;
    using Areas.Movement.ViewModels.Quantity;
    using Core.Shared;
    using Prsd.Core.Mapper;
    using Requests.Movement;

    public class MovementQuantityDataMap : IMap<MovementQuantityData, QuantityViewModel>
    {
        public QuantityViewModel Map(MovementQuantityData source)
        {
            string quantity = null;

            if (source.ThisMovementQuantity.HasValue
                && source.ThisMovementQuantity.Value > 0)
            {
                if (Math.Ceiling(source.ThisMovementQuantity.Value) == source.ThisMovementQuantity.Value)
                {
                    quantity = ((int)source.ThisMovementQuantity.Value).ToString("G");
                }
                else
                {
                    quantity = source.ThisMovementQuantity.Value.ToString("0,0.0000");
                }
            }

            return new QuantityViewModel
            {
                Units = source.Units,
                TotalUsed = source.TotalCurrentlyUsedQuantity,
                TotalAvailable = source.AvailableQuantity,
                TotalNotified = source.TotalNotifiedQuantity,
                Quantity = quantity,
                AvailableUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(source.Units).ToList()
            };
        }
    }
}