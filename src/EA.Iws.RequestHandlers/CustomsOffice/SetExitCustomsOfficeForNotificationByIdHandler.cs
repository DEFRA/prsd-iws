namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Notification;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class SetExitCustomsOfficeForNotificationByIdHandler : IRequestHandler<SetExitCustomsOfficeForNotificationById, Guid>
    {
        private readonly IwsContext context;

        public SetExitCustomsOfficeForNotificationByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetExitCustomsOfficeForNotificationById message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);
            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);

            notification.SetExitCustomsOffice(new ExitCustomsOffice(message.Name, 
                message.Address, 
                country));

            await context.SaveChangesAsync();

            return notification.ExitCustomsOffice.Id;
        }
    }
}
