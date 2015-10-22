namespace EA.Iws.RequestHandlers.Mappings.Movement.Summary
{
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

            var submittedStatusChange = source.StatusChanges.SingleOrDefault(sc => sc.Status == MovementStatus.Submitted);
            if (submittedStatusChange != null)
            {
                data.SubmittedDate = submittedStatusChange.ChangeDate;
            }

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
