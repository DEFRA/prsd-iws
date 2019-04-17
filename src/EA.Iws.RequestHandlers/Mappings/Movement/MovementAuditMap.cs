namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using EA.Iws.Domain.Movement;
    using Prsd.Core.Mapper;
    internal class MovementAuditMap : IMap<IEnumerable<MovementAudit>, ShipmentAuditData>
    {
        private readonly IMapper mapper;

        public MovementAuditMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ShipmentAuditData Map(IEnumerable<MovementAudit> movementAudits)
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
