namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Linq;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    internal class MovementTableDataRowMap : IMap<Movement, MovementTableDataRow>
    {
        public MovementTableDataRow Map(Movement source)
        {
            var data = new MovementTableDataRow
            {
                Id = source.Id,
                Number = source.Number,
                ShipmentDate = source.Date,
                HasShipped = source.HasShipped,
                IsShipmentActive = source.IsShipmentActive,
                Status = source.Status,
                SubmittedDate = (source.PrenotificationDate.HasValue)
                    ? source.PrenotificationDate.Value
                    : (DateTime?)null
            };

            if (source.Receipt != null)
            {
                data.ReceivedDate = source.Receipt.Date;
                data.Quantity = source.Receipt.QuantityReceived.Quantity;
                data.QuantityUnits = source.Receipt.QuantityReceived.Units;
            }

            if (source.Receipt == null && source.PartialRejection != null && source.PartialRejection.Count() > 0)
            {
                data.ReceivedDate = source.PartialRejection.FirstOrDefault().WasteReceivedDate;
                data.Quantity = source.PartialRejection.FirstOrDefault().ActualQuantity;
                data.QuantityUnits = source.PartialRejection.FirstOrDefault().ActualUnit;

                if (source.PartialRejection.FirstOrDefault().WasteDisposedDate != null)
                {
                    data.CompletedDate = source.PartialRejection.FirstOrDefault().WasteDisposedDate;
                }
            }

            if (source.CompletedReceipt != null)
            {
                data.CompletedDate = source.CompletedReceipt.Date;
            }

            return data;
        }
    }
}