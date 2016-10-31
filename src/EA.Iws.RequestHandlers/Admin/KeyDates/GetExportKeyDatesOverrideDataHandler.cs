namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using System.Threading.Tasks;
    using Core.Admin.KeyDates;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;

    public class GetExportKeyDatesOverrideDataHandler :
        IRequestHandler<GetExportKeyDatesOverrideData, KeyDatesOverrideData>
    {
        private readonly IKeyDatesOverrideRepository repository;

        public GetExportKeyDatesOverrideDataHandler(IKeyDatesOverrideRepository repository)
        {
            this.repository = repository;
        }

        public async Task<KeyDatesOverrideData> HandleAsync(GetExportKeyDatesOverrideData message)
        {
            return await repository.GetKeyDatesForNotification(message.NotificationId);
        }
    }
}