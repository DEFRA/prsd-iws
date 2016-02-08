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

        public GetFinancialGuaranteeDataByNotificationApplicationIdHandler(IwsContext context, IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData> financialGuaranteeMap)
        {
            this.context = context;
            this.financialGuaranteeMap = financialGuaranteeMap;
        }

        public async Task<FinancialGuaranteeData> HandleAsync(GetFinancialGuaranteeDataByNotificationApplicationId message)
        {
            var financialGuarantee =
                await context.FinancialGuarantees.SingleOrDefaultAsync(na => na.NotificationApplicationId == message.Id);
            var authority = (await context.NotificationApplications.SingleAsync(na => na.Id == message.Id)).CompetentAuthority;

            return financialGuaranteeMap.Map(financialGuarantee, authority);
        }
    }
}
