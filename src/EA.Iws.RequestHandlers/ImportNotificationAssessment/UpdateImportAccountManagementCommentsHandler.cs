namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Transactions;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class UpdateImportAccountManagementCommentsHandler : IRequestHandler<UpdateImportNotificationAssesmentComments, bool>
    {
        private readonly IImportNotificationTransactionRepository repository;

        public UpdateImportAccountManagementCommentsHandler(IImportNotificationTransactionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(UpdateImportNotificationAssesmentComments message)
        {
            await repository.UpdateById(message.NotificationId, message.Comment);
            return true;
        }
    }
}
