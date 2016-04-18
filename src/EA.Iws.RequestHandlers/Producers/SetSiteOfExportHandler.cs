namespace EA.Iws.RequestHandlers.Producers
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Producers;

    internal class SetSiteOfExportHandler : IRequestHandler<SetSiteOfExport, Guid>
    {
        private readonly IProducerRepository repository;
        private readonly IwsContext context;

        public SetSiteOfExportHandler(IwsContext context,
            IProducerRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(SetSiteOfExport command)
        {
            var producers = await repository.GetByNotificationId(command.NotificationId);

            producers.SetProducerAsSiteOfExport(command.ProducerId);

            await context.SaveChangesAsync();

            return command.NotificationId;
        }
    }
}