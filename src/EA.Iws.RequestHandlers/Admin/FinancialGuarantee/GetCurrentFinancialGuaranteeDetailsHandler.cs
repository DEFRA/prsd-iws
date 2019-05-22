namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class GetCurrentFinancialGuaranteeDetailsHandler :
        IRequestHandler<GetCurrentFinancialGuaranteeDetails, FinancialGuaranteeData>
    {
        private readonly IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationAssessmentRepository assessmentRepository;

        public GetCurrentFinancialGuaranteeDetailsHandler(
            IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap,
            IFinancialGuaranteeRepository financialGuaranteeRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            INotificationAssessmentRepository assessmentRepository)
        {
            this.financialGuaranteeMap = financialGuaranteeMap;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<FinancialGuaranteeData> HandleAsync(GetCurrentFinancialGuaranteeDetails message)
        {
            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(message.NotificationId);
            var financialGuarantee = financialGuaranteeCollection.GetLatestFinancialGuarantee();
            var authority = (await notificationApplicationRepository.GetById(message.NotificationId)).CompetentAuthority;
            var notificationStatus = await assessmentRepository.GetStatusByNotificationId(message.NotificationId);

            var result = financialGuaranteeMap.Map(financialGuarantee, authority);

            result.IsNotificationStatusRecordable = IsRecordableStatus(notificationStatus);

            return result;
        }

        private static bool IsRecordableStatus(NotificationStatus status)
        {
            var invalidStatuses = new[]
            {
                NotificationStatus.Withdrawn,
                NotificationStatus.ConsentWithdrawn,
                NotificationStatus.Objected,
                NotificationStatus.FileClosed
            };

            return !invalidStatuses.Any(x => x == status);
        }
    }
}