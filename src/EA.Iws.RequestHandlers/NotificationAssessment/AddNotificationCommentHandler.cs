namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class AddNotificationCommentHandler : IRequestHandler<AddNotificationComment, bool>
    {
        private readonly INotificationCommentRepository repository;

        public AddNotificationCommentHandler(INotificationCommentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(AddNotificationComment message)
        {
            NotificationComment comment = new NotificationComment(message.NotificationId, message.UserId, message.Comment, message.ShipmentNumber, message.DateAdded);

            return await this.repository.Add(comment);
        }
    }
}
