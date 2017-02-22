namespace EA.Iws.Api
{
    using DataAccess.Jobs;
    using Hangfire;

    internal static class HangFireJobsSetup
    {
        public static void Configure()
        {
            RecurringJob.AddOrUpdate<UpdateReportCache>(x => x.Execute(), Cron.Daily(3));
        }
    }
}