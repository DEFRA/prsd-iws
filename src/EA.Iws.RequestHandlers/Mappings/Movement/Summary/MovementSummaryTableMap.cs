namespace EA.Iws.RequestHandlers.Mappings.Movement.Summary
{
    using System;
    using System.Linq;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    internal class MovementSummaryTableMap : IMap<Movement, MovementSummaryTableData>
    {
        public MovementSummaryTableData Map(Movement source)
        {
            var data = new MovementSummaryTableData();

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
                data.Quantity = source.Receipt.Quantity;
            }

            if (source.CompletedReceipt != null)
            {
                data.CompletedDate = source.CompletedReceipt.Date;
            }

            data.QuantityUnits = source.Units;

            return data;
        }
    }
}
