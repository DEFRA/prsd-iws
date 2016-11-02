namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;

    internal class SetimportKeyDatesOverrideHandler : IRequestHandler<SetImportKeyDatesOverride, Unit>
    {
        private readonly IKeyDatesOverrideRepository repository;

        public SetimportKeyDatesOverrideHandler(IKeyDatesOverrideRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> HandleAsync(SetImportKeyDatesOverride message)
        {
            await repository.SetKeyDatesForNotification(message.Data);
            return Unit.Value;
        }
    }
}