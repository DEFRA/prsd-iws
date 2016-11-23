namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Admin;
    using Core.Notification;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    public class GetFinancialGuaranteeDataByNotificationApplicationIdHandler : IRequestHandler<GetFinancialGuaranteeDataByNotificationApplicationId, FinancialGuaranteeData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap;
        private readonly IFinancialGuaranteeRepository repository;

        public GetFinancialGuaranteeDataByNotificationApplicationIdHandler(IwsContext context, 
            IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap, 
            IFinancialGuaranteeRepository repository)
        {
            this.context = context;
            this.financialGuaranteeMap = financialGuaranteeMap;
            this.repository = repository;
        }

        public async Task<FinancialGuaranteeData> HandleAsync(GetFinancialGuaranteeDataByNotificationApplicationId message)
        {
            var financialGuaranteeCollection = await repository.GetByNotificationId(message.NotificationId);
            var financialGuarantee = financialGuaranteeCollection.GetFinancialGuarantee(message.FinancialGuaranteeId);

            var authority = (await context.NotificationApplications.SingleAsync(na => na.Id == message.NotificationId)).CompetentAuthority;

            return financialGuaranteeMap.Map(financialGuarantee, authority);
        }
    }
}
