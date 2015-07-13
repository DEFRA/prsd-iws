namespace EA.Iws.RequestHandlers.Producers
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class SetSiteOfExportHandler : IRequestHandler<SetSiteOfExport, Guid>
    {
        private readonly IwsContext context;

        public SetSiteOfExportHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetSiteOfExport command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            notification.SetProducerAsSiteOfExport(command.ProducerId);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}