namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Exporter;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetExporterDetailsHandler : IRequestHandler<SetExporterDetails, Unit>
    {
        private readonly IwsContext context;
        private readonly IExporterRepository repository;

        public SetExporterDetailsHandler(IExporterRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetExporterDetails message)
        {
            var exporter = await repository.GetByNotificationId(message.NotificationId);
            var contact = ValueObjectInitializer.CreateContact(message.Exporter.Contact);
            var business = ValueObjectInitializer.CreateBusiness(message.Exporter.Business);
            exporter.UpdateContactAndBusiness(contact, business);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}