namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using DataAccess;
    using EA.Iws.Domain.NotificationApplication;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using System.Linq;
    using System.Threading.Tasks;

    internal class SetFacilityDetailsHandler : IRequestHandler<SetFacilityDetails, Unit>
    {
        private readonly IwsContext context;
        private readonly IFacilityRepository repository;

        public SetFacilityDetailsHandler(IFacilityRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetFacilityDetails message)
        {
            var facilityCollection = await repository.GetByNotificationId(message.NotificationId);
            var facility = facilityCollection.Facilities.FirstOrDefault();
            var contact = ValueObjectInitializer.CreateContact(message.FacilityData.Contact);
            var business = ValueObjectInitializer.CreateBusiness(message.FacilityData.Business);
            var address = ValueObjectInitializer.CreateAddress(message.FacilityData.Address, message.FacilityData.Address.CountryName);

            facility.UpdateContactAndBusiness(contact, business, address);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
