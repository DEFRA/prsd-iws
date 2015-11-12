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
            var data = new MovementTableDataRow();

            data.Number = source.Number;
            data.ShipmentDate = source.Date;
            data.Status = source.Status;

            data.SubmittedDate = source.StatusChanges
                .Where(sc => sc.Status == MovementStatus.Submitted)
                .Select(sc => (DateTime?)sc.ChangeDate)
                .SingleOrDefault();

            if (source.Receipt != null)
            {
                data.ReceivedDate = source.Receipt.Date;
                data.Quantity = source.Receipt.QuantityReceived.Quantity;
                data.QuantityUnits = source.Receipt.QuantityReceived.Units;
            }

            if (source.CompletedReceipt != null)
            {
                data.CompletedDate = source.CompletedReceipt.Date;
            }

            return data;
        }
    }
}