namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using EA.Iws.Domain.ImportNotificationAssessment;
    using EA.Iws.Requests.ImportNotificationAssessment;
    using EA.Prsd.Core.Mediator;

    internal class DeleteImportNotificationCommentHandler : IRequestHandler<DeleteImportNotificationComment, bool>
    {
        private readonly IImportNotificationCommentRepository repository;

        public DeleteImportNotificationCommentHandler(IImportNotificationCommentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(DeleteImportNotificationComment message)
        {
            var result = await this.repository.Delete(message.CommentId);

            return result;
        }
    }
}
