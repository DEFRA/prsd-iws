namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class CheckImportNotificationHasCommentsHandler : IRequestHandler<CheckImportNotificationHasComments, bool>
    {
        private readonly IImportNotificationCommentRepository repository;

        public CheckImportNotificationHasCommentsHandler(IImportNotificationCommentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CheckImportNotificationHasComments message)
        {
            var commentsCount = await repository.GetCommentsCountForImportNotification(message.ImportNotificationId);

            return commentsCount > 0 ? true : false;
        }
    }
}
