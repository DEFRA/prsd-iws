namespace EA.Iws.RequestHandlers.Notification
{
    using System.Text;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Notification;

    internal class NotificationNumberGenerator : INotificationNumberGenerator
    {
        private const string NotificationNumberSequenceFormat = "[Notification].[{0}NotificationNumber]";
        private readonly IwsContext context;

        public NotificationNumberGenerator(IwsContext context)
        {
            this.context = context;
        }

        public async Task<int> GetNextNotificationNumber(UKCompetentAuthority competentAuthority)
        {
            return await context.Database.SqlQuery<int>(CreateSqlQuery(competentAuthority)).SingleAsync();
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