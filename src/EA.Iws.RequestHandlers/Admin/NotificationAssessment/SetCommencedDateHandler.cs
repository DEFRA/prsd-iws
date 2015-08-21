namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetCommencedDateHandler : IRequestHandler<SetCommencedDate, bool>
    {
        private readonly IwsContext context;

        public SetCommencedDateHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetCommencedDate message)
        {
            var assessment =
                await
                    context.NotificationAssessments.SingleAsync(
                        p => p.NotificationApplicationId == message.NotificationId);

            assessment.SetCommencementDate(message.CommencementDate, message.NameOfOfficer);

            await context.SaveChangesAsync();

            return true;
        }
    }
}