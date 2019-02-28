namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class AddImportNotificationCommentHandler : IRequestHandler<AddImportNotificationComment, bool>
    {
        private readonly IImportNotificationCommentRepository repository;

        public AddImportNotificationCommentHandler(IImportNotificationCommentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(AddImportNotificationComment message)
        {
            ImportNotificationComment comment = new ImportNotificationComment(message.ImportNotificationId, message.UserId, message.Comment, message.ShipmentNumber, message.DateAdded);

           return await this.repository.Add(comment);
        }
    }
}
