namespace EA.Iws.RequestHandlers.Facilities
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Facilities;

    internal class SetActualSiteOfTreatmentHandler : IRequestHandler<SetActualSiteOfTreatment, Guid>
    {
        private readonly IwsContext context;

        public SetActualSiteOfTreatmentHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetActualSiteOfTreatment command)
        {
            var notification = await context.GetNotificationApplication(command.NotificationId);
            notification.SetFacilityAsSiteOfTreatment(command.FacilityId);

            await context.SaveChangesAsync();

            return notification.Id;
        }
    }
}