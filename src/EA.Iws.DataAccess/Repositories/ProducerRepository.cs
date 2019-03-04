namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Core.Reports.Producer;
    using Domain.NotificationApplication;
    using Domain.Security;

    internal class ProducerRepository : IProducerRepository
    {
        private readonly INotificationApplicationAuthorization authorization;
        private readonly IwsContext context;

        public ProducerRepository(IwsContext context,
            INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(ProducerCollection producerCollection)
        {
            context.Producers.Add(producerCollection);
        }

        public async Task<ProducerCollection> GetByMovementId(Guid movementId)
        {
            var notificationId =
                await context.Movements.Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            return await GetByNotificationId(notificationId);
        }

        public async Task<ProducerCollection> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            return await context.Producers.SingleAsync(x => x.NotificationId == notificationId);
        }

        public async Task<IEnumerable<ProducerData>> GetProducerReport(ProducerReportDates dateType, 
            DateTime from,
            DateTime to,
            ProducerReportTextFields? textFieldType, 
            TextFieldOperator? operatorType, 
            string textSearch,
            UKCompetentAuthority competentAuthority)
        {
            var textFilter = TextFilterHelper.GetTextFilter(textFieldType, operatorType, textSearch);

            return await context.Database.SqlQuery<ProducerData>(
                @"SELECT DISTINCT
	                [NotificationNumber]
	                ,[NotifierName]
	                ,[ProducerName]
	                ,[ProducerAddress1]
	                ,[ProducerAddress2]
	                ,[ProducerTownOrCity]
	                ,[ProducerPostCode]
	                ,[SiteOfExport]
	                ,[LocalArea]
	                ,[WasteType]
	                ,[NotificationStatus]
	                ,[ConsigneeName]
                FROM 
	                [Reports].[Producers]
                WHERE
	                [CompetentAuthorityId] = @competentAuthority
	                AND (@dateType = 'NotificationReceivedDate' AND  [NotificationReceivedDate] BETWEEN @from AND @to
                                         OR @dateType = 'ConsentFrom' AND  [ConsentFrom] BETWEEN @from AND @to
                                         OR @dateType = 'ConsentTo' AND  [ConsentTo] BETWEEN @from AND @to
                                         OR @dateType = 'ReceivedDate' AND [MovementReceivedDate] BETWEEN @from AND @to
                                         OR @dateType = 'CompletedDate' AND [MovementCompletedDate] BETWEEN @from AND @to)
                    AND (@textFilter IS NULL OR @textFilter = @textFilter)",
                new SqlParameter("@dateType", dateType.ToString()),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@textFilter",
                    (!string.IsNullOrEmpty(textFilter) ? (object)textFilter : DBNull.Value)),
                new SqlParameter("@competentAuthority", (int)competentAuthority)).ToArrayAsync();
        }
    }
}