namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class UpdateExportAccountManagementCommentsHandler : IRequestHandler<UpdateExportNotificationAssementComments, bool>
    {
        private readonly INotificationTransactionRepository repository;

        public UpdateExportAccountManagementCommentsHandler(INotificationTransactionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(UpdateExportNotificationAssementComments message)
        {
            await repository.UpdateById(message.NotificationId, message.Comment);
            return true;
        }
    }
}
