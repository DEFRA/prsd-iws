namespace EA.Iws.DataAccess.Repositories.Imports
{
    using Domain.ImportNotificationAssessment;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class KeyDatesRepository : IKeyDatesRepository
    {
        private readonly IwsContext context;

        public KeyDatesRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task SetNameOfOfficerForNotification(Guid notificationAssessmentId, string nameOfOfficer)
        {
            await context.Database.ExecuteSqlCommandAsync(@"
                UPDATE [ImportNotification].[NotificationDates]
                SET NameOfOfficer = @NameOfOfficer 
                WHERE NotificationAssessmentId = @NotificationAssessmentId",
                new SqlParameter("@NotificationAssessmentId", notificationAssessmentId),
                new SqlParameter("@NameOfOfficer", nameOfOfficer));
        }
    }
}