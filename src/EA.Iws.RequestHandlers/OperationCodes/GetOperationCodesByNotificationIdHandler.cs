namespace EA.Iws.RequestHandlers.OperationCodes
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.OperationCodes;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.OperationCodes;

    internal class GetOperationCodesByNotificationIdHandler : IRequestHandler<GetOperationCodesByNotificationId, IList<OperationCodeData>>
    {
        private readonly IwsContext context;

        public GetOperationCodesByNotificationIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<OperationCodeData>> HandleAsync(GetOperationCodesByNotificationId query)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == query.NotificationId);
            var codesList = new List<OperationCodeData>();

            foreach (var operationInfo in notification.OperationInfos)
            {
                var ocd = new OperationCodeData
                {
                    Code = operationInfo.OperationCode.DisplayName, 
                    Value = operationInfo.OperationCode.Value
                };
                codesList.Add(ocd);
            }

            return codesList;
        }
    }
}
