namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication.Importer;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class SetImporterDetailsHandler : IRequestHandler<SetImporterDetails, Unit>
    {
        private readonly IwsContext context;
        private readonly IImporterRepository repository;

        public SetImporterDetailsHandler(IImporterRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetImporterDetails message)
        {
            var importer = await repository.GetByNotificationId(message.NotificationId);
            var contact = ValueObjectInitializer.CreateContact(message.Importer.Contact);
            var business = ValueObjectInitializer.CreateBusiness(message.Importer.Business);
            var address = ValueObjectInitializer.CreateAddress(message.Importer.Address, message.Importer.Address.CountryName);

            importer.UpdateContactAndBusiness(contact, business, address);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}