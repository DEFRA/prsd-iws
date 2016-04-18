namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment.Decision;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class ObjectToImportNotificationHandler : IRequestHandler<ObjectToImportNotification, bool>
    {
        private readonly ObjectImportNotification objectImportNotification;
        private readonly ImportNotificationContext context;

        public ObjectToImportNotificationHandler(ObjectImportNotification objectImportNotification, ImportNotificationContext context)
        {
            this.objectImportNotification = objectImportNotification;
            this.context = context;
        }

        public async Task<bool> HandleAsync(ObjectToImportNotification message)
        {
            await objectImportNotification.Object(message.Id, message.Date, message.ReasonsForObjection);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
