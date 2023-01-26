namespace EA.Iws.DataAccess.Repositories
{
    using EA.Iws.Domain;
    using EA.Prsd.Core.Domain;
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    public class ArchiveNotificationRepository : IArchiveNotificationRepository
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public ArchiveNotificationRepository(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<string> ArchiveNotificationAsync(Guid notificationId)
        {
            var res = "false";
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    res = await context.Database.SqlQuery<string>
                        (@"[Notification].[uspArchiveNotification] @NotificationId, @CurrentUserId",
                        new SqlParameter("@NotificationId", notificationId),
                        new SqlParameter("@CurrentUserId", userContext.UserId)).SingleAsync();
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    await LogArchiveSqlExceptionAsync(ex, notificationId);
                }
            }
            return res;
        }

        private async Task LogArchiveSqlExceptionAsync(Exception ex, Guid notificationId)
        {
            try
            {
                var errorXml = GetExceptionAsXml(ex, notificationId);
                var currentUserName = userContext.Principal.Identity.Name;
                await context.Database.ExecuteSqlCommandAsync(
                    "ELMAH_LogError @ErrorId, @Application, @Host, @Type, @Source, @Message, @User, @AllXml, @StatusCode, @TimeUtc",
                        new SqlParameter("@ErrorId", Guid.NewGuid()),
                        new SqlParameter("@Application", "IwsArchiveNotification"),
                        new SqlParameter("@Host", "Idk"),
                        new SqlParameter("@Type", ex.GetType().FullName),
                        new SqlParameter("@Source", ex.Source),
                        new SqlParameter("@Message", ex.Message),
                        new SqlParameter("@User", currentUserName),
                        new SqlParameter("@AllXml", errorXml),
                        new SqlParameter("@StatusCode", "500"),
                        new SqlParameter("@TimeUtc", DateTime.UtcNow));
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        private string GetExceptionAsXml(Exception ex, Guid notificationId)
        {
            var err = new ElmahErrorXmlFormat()
            {
                ExceptionMessage = ex.Message,
                ExceptionType = ex.GetType().Name,
                Message = "The notification: " + notificationId + " was not able to be archived",
                StackTrace = ex.StackTrace
            };
            var apiError = new XmlSerializer(typeof(ElmahErrorXmlFormat));

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    apiError.Serialize(writer, err);
                    return sww.ToString();
                }
            }
        }

        public class ElmahErrorXmlFormat
        {
            public string Message { get; set; }

            public string ExceptionMessage { get; set; }

            public string ExceptionType { get; set; }

            public string StackTrace { get; set; }
        }
    }
}