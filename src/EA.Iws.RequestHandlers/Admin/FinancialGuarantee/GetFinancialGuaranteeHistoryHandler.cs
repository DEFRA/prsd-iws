namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.Notification;
    using Domain.FinancialGuarantee;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class GetFinancialGuaranteeHistoryHandler :
        IRequestHandler<GetFinancialGuaranteeHistory, FinancialGuaranteeData[]>
    {
        private readonly IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap;
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public GetFinancialGuaranteeHistoryHandler(
            IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap,
            IFinancialGuaranteeRepository financialGuaranteeRepository,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.financialGuaranteeMap = financialGuaranteeMap;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<FinancialGuaranteeData[]> HandleAsync(GetFinancialGuaranteeHistory message)
        {
            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(message.NotificationId);
            var authority = (await notificationApplicationRepository.GetById(message.NotificationId)).CompetentAuthority;

            var latestFinancialGuarantee = financialGuaranteeCollection.GetLatestFinancialGuarantee();
            var latestFinancialGuaranteeId = latestFinancialGuarantee == null ? Guid.Empty : latestFinancialGuarantee.Id;

            return financialGuaranteeCollection.FinancialGuarantees
                .Where(fg => fg.Id != latestFinancialGuaranteeId)
                .Select(fg => financialGuaranteeMap.Map(fg, authority))
                .ToArray();
        }
    }
}