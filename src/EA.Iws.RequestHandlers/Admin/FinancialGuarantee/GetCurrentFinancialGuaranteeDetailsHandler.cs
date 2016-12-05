namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Notification;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class GetCurrentFinancialGuaranteeDetailsHandler :
        IRequestHandler<GetCurrentFinancialGuaranteeDetails, FinancialGuaranteeData>
    {
        private readonly IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public GetCurrentFinancialGuaranteeDetailsHandler(IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap,
            IFinancialGuaranteeRepository financialGuaranteeRepository,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.financialGuaranteeMap = financialGuaranteeMap;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<FinancialGuaranteeData> HandleAsync(GetCurrentFinancialGuaranteeDetails message)
        {
            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(message.NotificationId);
            var financialGuarantee = financialGuaranteeCollection.GetLatestFinancialGuarantee();
            var authority = (await notificationApplicationRepository.GetById(message.NotificationId)).CompetentAuthority;

            return financialGuaranteeMap.Map(financialGuarantee, authority);
        }
    }
}