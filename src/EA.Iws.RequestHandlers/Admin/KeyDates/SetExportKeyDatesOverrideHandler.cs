namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;

    internal class SetExportKeyDatesOverrideHandler : IRequestHandler<SetExportKeyDatesOverride, Unit>
    {
        private readonly IKeyDatesOverrideRepository repository;

        public SetExportKeyDatesOverrideHandler(IKeyDatesOverrideRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> HandleAsync(SetExportKeyDatesOverride message)
        {
            await repository.SetKeyDatesForNotification(message.Data);
            return Unit.Value;
        }
    }
}