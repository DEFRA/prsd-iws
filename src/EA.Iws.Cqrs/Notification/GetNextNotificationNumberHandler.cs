namespace EA.Iws.Cqrs.Notification
{
    using System.Text;
    using System.Threading.Tasks;
    using Core.Cqrs;
    using DataAccess;
    using Domain;

    public class GetNextNotificationNumberHandler : IQueryHandler<GetNextNotificationNumber, int>
    {
        private const string NotificationNumberSequenceFormat = "[Notification].[{0}NotificationNumber]";
        private readonly IwsContext context;

        public GetNextNotificationNumberHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<int> ExecuteAsync(GetNextNotificationNumber query)
        {
            return await context.Database.SqlQuery<int>(CreateSqlQuery(query.CompetentAuthority)).SingleAsync();
        }

        private static string CreateSqlQuery(UKCompetentAuthority competentAuthority)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT NEXT VALUE FOR ");
            stringBuilder.AppendFormat(NotificationNumberSequenceFormat, competentAuthority.ShortName);
            return stringBuilder.ToString();
        }
    }
}