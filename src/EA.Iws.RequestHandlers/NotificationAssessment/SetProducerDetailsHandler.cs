namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using DataAccess;
    using EA.Iws.Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using System.Linq;
    using System.Threading.Tasks;

    internal class SetProducerDetailsHandler : IRequestHandler<SetProducerDetails, Unit>
    {
        private readonly IwsContext context;
        private readonly IProducerRepository repository;

        public SetProducerDetailsHandler(IProducerRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetProducerDetails message)
        {
            var producerList = await repository.GetByNotificationId(message.NotificationId);
            var producer = producerList.Producers.FirstOrDefault();
            var contact = ValueObjectInitializer.CreateContact(message.Producer.Contact);
            var business = ValueObjectInitializer.CreateBusiness(message.Producer.Business);
            var producerBusiness = ProducerBusiness.CreateProducerBusiness(business.Name, business.Type, business.RegistrationNumber, business.OtherDescription);
            var address = ValueObjectInitializer.CreateAddress(message.Producer.Address, message.Producer.Address.CountryName);

            producer.UpdateContactAndBusiness(contact, producerBusiness, address);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
