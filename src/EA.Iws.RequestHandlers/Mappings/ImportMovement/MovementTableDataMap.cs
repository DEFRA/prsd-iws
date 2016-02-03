namespace EA.Iws.RequestHandlers.Mappings.ImportMovement
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.ImportNotificationMovements;
    using Prsd.Core.Mapper;

    internal class MovementTableDataMap : IMap<IEnumerable<Domain.ImportMovement.MovementTableData>, IEnumerable<Core.ImportNotificationMovements.MovementTableData>>
    {
        public IEnumerable<MovementTableData> Map(IEnumerable<Domain.ImportMovement.MovementTableData> source)
        {
            return source.Select(i => new MovementTableData
            {
                Number = i.Number,
                PreNotification = i.PreNotification,
                ShipmentDate = i.ShipmentDate,
                Received = i.Received,
                Quantity = i.Quantity,
                Unit = i.Unit,
                Rejected = i.Rejected,
                RecoveredOrDisposedOf = i.RecoveredOrDisposedOf,
                Status = i.Status
            }).ToList();
        }
    }
}
