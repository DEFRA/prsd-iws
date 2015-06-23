namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.TransportRoute;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class SetEntryCustomsOfficeForNotificationByIdHandler : IRequestHandler<SetEntryCustomsOfficeForNotificationById, Guid>
    {
        private readonly IwsContext context;

        public SetEntryCustomsOfficeForNotificationByIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetEntryCustomsOfficeForNotificationById message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);
            var country = await context.Countries.SingleAsync(c => c.Id == message.CountryId);

            notification.SetEntryCustomsOffice(new EntryCustomsOffice(message.Name, 
                message.Address, 
                country));

            await context.SaveChangesAsync();

            return notification.EntryCustomsOffice.Id;
        }
    }
}
