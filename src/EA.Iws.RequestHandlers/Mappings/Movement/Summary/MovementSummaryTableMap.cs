namespace EA.Iws.RequestHandlers.Mappings.Movement.Summary
{
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
            if (source.Receipt != null)
            {
                data.ReceivedDate = source.Receipt.Date;
                data.Quantity = source.Receipt.Quantity;

                if (source.Receipt.OperationReceipt != null)
                {
                    data.CompletedDate = source.Receipt.OperationReceipt.Date;
                }
            }
            data.QuantityUnits = source.Units;
            
            return data;
        }
    }
}
