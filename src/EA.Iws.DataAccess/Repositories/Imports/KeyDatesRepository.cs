namespace EA.Iws.DataAccess.Repositories
{
    using Domain.NotificationAssessment;
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
                UPDATE [Notification].[NotificationDates]
                SET NameOfOfficer = @NameOfOfficer 
                WHERE NotificationAssessmentId = @NotificationAssessmentId",
                new SqlParameter("@NotificationAssessmentId", notificationAssessmentId),
                new SqlParameter("@NameOfOfficer", nameOfOfficer));
        }
    }
}