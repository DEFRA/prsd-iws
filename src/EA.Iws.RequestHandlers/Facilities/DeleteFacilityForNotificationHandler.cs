namespace EA.Iws.RequestHandlers.Facilities
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class DeleteFacilityForNotificationHandler : IRequestHandler<DeleteFacilityForNotification, bool>
    {
        private readonly IwsContext context;
        private readonly IFacilityRepository facilityRepository;

        public DeleteFacilityForNotificationHandler(IwsContext context, IFacilityRepository facilityRepository)
        {
            this.context = context;
            this.facilityRepository = facilityRepository;
        }

        public async Task<bool> HandleAsync(DeleteFacilityForNotification query)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(query.NotificationId);
            facilityCollection.RemoveFacility(query.FacilityId);
            await context.SaveChangesAsync();
            return true;
        }
    }
}