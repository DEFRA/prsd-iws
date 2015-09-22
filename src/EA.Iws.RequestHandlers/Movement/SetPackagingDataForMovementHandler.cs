namespace EA.Iws.RequestHandlers.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.PackagingType;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class SetPackagingDataForMovementHandler : IRequestHandler<SetPackagingDataForMovement, Guid>
    {
        private readonly IwsContext context;

        public SetPackagingDataForMovementHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetPackagingDataForMovement message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.MovementId);
            var notification = await context.GetNotificationApplication(movement.NotificationId);
            var packagingInfos = notification.PackagingInfos
                .Where(info => PackagingTypesAsInt(message.PackagingTypes)
                .Contains(info.PackagingType.Value));

            movement.SetPackagingInfos(packagingInfos);

            await context.SaveChangesAsync();

            return new Guid();
        }

        private IEnumerable<int> PackagingTypesAsInt(IEnumerable<PackagingType> collection)
        {
            return collection.Select(item => (int)item);
        }
    }
}
