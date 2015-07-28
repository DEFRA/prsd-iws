namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    public class GetFinancialGuaranteeDataByNotificationApplicationIdHandler : IRequestHandler<GetFinancialGuaranteeDataByNotificationApplicationId, FinancialGuaranteeData>
    {
        private readonly IwsContext context;
        private readonly IMap<FinancialGuarantee, FinancialGuaranteeData> financialGuaranteeMap;

        public GetFinancialGuaranteeDataByNotificationApplicationIdHandler(IwsContext context, IMap<FinancialGuarantee, FinancialGuaranteeData> financialGuaranteeMap)
        {
            this.context = context;
            this.financialGuaranteeMap = financialGuaranteeMap;
        }

        public async Task<FinancialGuaranteeData> HandleAsync(GetFinancialGuaranteeDataByNotificationApplicationId message)
        {
            var assessment =
                await context.NotificationAssessments.SingleAsync(na => na.NotificationApplicationId == message.Id);

            return financialGuaranteeMap.Map(assessment.FinancialGuarantee);
        }
    }
}
