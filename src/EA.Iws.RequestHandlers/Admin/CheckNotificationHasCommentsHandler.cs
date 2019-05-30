namespace EA.Iws.RequestHandlers.Admin
{
    using System.Threading.Tasks;
    using EA.Iws.Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    internal class CheckNotificationHasCommentsHandler : IRequestHandler<CheckNotificationHasComments, bool>
    {
        private readonly INotificationCommentRepository repository;

        public CheckNotificationHasCommentsHandler(INotificationCommentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CheckNotificationHasComments message)
        {
            var commentsCount = await repository.GetCommentsCountForNotification(message.NotificationId);

            return commentsCount > 0;
        }
    }
}
