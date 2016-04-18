namespace EA.Iws.RequestHandlers.Facilities
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class SetActualSiteOfTreatmentHandler : IRequestHandler<SetActualSiteOfTreatment, Guid>
    {
        private readonly IwsContext context;
        private readonly IFacilityRepository facilityRepository;

        public SetActualSiteOfTreatmentHandler(IwsContext context, IFacilityRepository facilityRepository)
        {
            this.context = context;
            this.facilityRepository = facilityRepository;
        }

        public async Task<Guid> HandleAsync(SetActualSiteOfTreatment command)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(command.NotificationId);
            facilityCollection.SetFacilityAsSiteOfTreatment(command.FacilityId);

            await context.SaveChangesAsync();

            return facilityCollection.NotificationId;
        }
    }
}