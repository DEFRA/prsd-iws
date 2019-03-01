namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;

    internal class DeleteNotificationCommentHandler : IRequestHandler<DeleteNotificationComment, bool>
    {
        private readonly INotificationCommentRepository repository;

        public DeleteNotificationCommentHandler(INotificationCommentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteNotificationComment message)
        {
            var result = await this.repository.Delete(message.CommentId);

            return result;
        }
    }
}
