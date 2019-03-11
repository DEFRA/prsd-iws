namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Domain.Reports;
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

        public async Task<IEnumerable<ProducerData>> GetProducerReport(ProducerReportDates dateType,
            DateTime from,
            DateTime to,
            ProducerReportTextFields? textFieldType,
            TextFieldOperator? operatorType,
            string textSearch,
            UKCompetentAuthority competentAuthority)
        {
            var textFilter = TextFilterHelper.GetTextFilter(textFieldType, operatorType, textSearch);
            textFilter = !string.IsNullOrEmpty(textFilter) ? string.Format("AND {0}", textFilter) : string.Empty;

            var query = @"SELECT DISTINCT
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
                    {0}";

            return await context.Database.SqlQuery<ProducerData>(string.Format(query, textFilter),
                new SqlParameter("@dateType", dateType.ToString()),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@competentAuthority", (int)competentAuthority)).ToArrayAsync();
        }
    }
}
