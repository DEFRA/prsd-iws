namespace EA.Iws.RequestHandlers.Mappings.ImportMovement
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    internal class MovementAuditMap : IMap<IEnumerable<ImportMovementAudit>, ShipmentAuditData>
    {
        private readonly IMapper mapper;

        public MovementAuditMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ShipmentAuditData Map(IEnumerable<ImportMovementAudit> movementAudits)
        {
            return new ShipmentAuditData
            {
                TableData = movementAudits
                    .Select(shipmentAudit => mapper.Map<ShipmentAuditRecord>(shipmentAudit))
                    .OrderByDescending(x => x.ShipmentNumber)
                    .ThenByDescending(x => x.DateAdded)
                    .ToList()
            };
        }
    }
}
