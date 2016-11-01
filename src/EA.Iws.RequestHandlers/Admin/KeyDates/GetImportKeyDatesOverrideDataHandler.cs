namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using System.Threading.Tasks;
    using Core.Admin.KeyDates;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;

    public class GetImportKeyDatesOverrideDataHandler :
        IRequestHandler<GetImportKeyDatesOverrideData, KeyDatesOverrideData>
    {
        private readonly IKeyDatesOverrideRepository repository;

        public GetImportKeyDatesOverrideDataHandler(IKeyDatesOverrideRepository repository)
        {
            this.repository = repository;
        }

        public async Task<KeyDatesOverrideData> HandleAsync(GetImportKeyDatesOverrideData message)
        {
            return await repository.GetKeyDatesForNotification(message.NotificationId);
        }
    }
}