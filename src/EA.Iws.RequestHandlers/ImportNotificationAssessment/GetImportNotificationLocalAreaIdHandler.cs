namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportNotificationLocalAreaIdHandler : IRequestHandler<GetImportNotificationLocalAreaId, Guid>
    {
        private readonly IImportNotificationAssessmentRepository repository;

        public GetImportNotificationLocalAreaIdHandler(IImportNotificationAssessmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(GetImportNotificationLocalAreaId message)
        {
            var assessment = await repository.GetByNotification(message.NotificationId);

            return assessment.LocalAreaId.GetValueOrDefault();
        }
    }
}
