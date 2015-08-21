namespace EA.Iws.RequestHandlers.IntendedShipments
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.IntendedShipments;

    internal class SetIntendedShipmentInfoForNotificationHandler : IRequestHandler<SetIntendedShipmentInfoForNotification, Guid>
    {
        private readonly IwsContext context;

        public SetIntendedShipmentInfoForNotificationHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetIntendedShipmentInfoForNotification command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);

            notification.SetShipmentInfo(command.StartDate, command.EndDate, command.NumberOfShipments,
                command.Quantity, command.Units);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}