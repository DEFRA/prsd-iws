namespace EA.Iws.RequestHandlers.Notification
{
    using System.Text;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.Notification;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;
    [AutoRegister]
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
            stringBuilder.AppendFormat(NotificationNumberSequenceFormat, EnumHelper.GetShortName(competentAuthority));
            return stringBuilder.ToString();
        }
    }
}