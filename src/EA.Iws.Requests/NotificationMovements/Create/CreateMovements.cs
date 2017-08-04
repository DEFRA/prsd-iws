namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.PackagingType;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovementsExternal)]
    public class CreateMovements : IRequest<Guid[]>
    {
        public CreateMovements(Guid notificationId, int numberToCreate, DateTime actualMovementDate,
            decimal quantity, ShipmentQuantityUnits units, IList<PackagingType> packagingTypes)
        {
            NotificationId = notificationId;
            NumberToCreate = numberToCreate;
            ActualMovementDate = actualMovementDate;
            Quantity = quantity;
            Units = units;
            PackagingTypes = packagingTypes;
        }

        public Guid NotificationId { get; private set; }

        public int NumberToCreate { get; private set; }

        public DateTime ActualMovementDate { get; private set; }

        public decimal Quantity { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }

        public IList<PackagingType> PackagingTypes { get; private set; }
    }
}