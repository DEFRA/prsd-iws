namespace EA.Iws.RequestHandlers.NotificationMovements
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    internal class GetWorkingDaysUntilHandler : IRequestHandler<GetWorkingDaysUntil, int>
    {
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public GetWorkingDaysUntilHandler(IWorkingDayCalculator workingDayCalculator, INotificationApplicationRepository notificationApplicationRepository)
        {
            this.workingDayCalculator = workingDayCalculator;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<int> HandleAsync(GetWorkingDaysUntil message)
        {
            var ca = (await notificationApplicationRepository.GetById(message.NotificationId)).CompetentAuthority;
            return workingDayCalculator.GetWorkingDays(SystemTime.UtcNow, message.Date, false, ca);
        }
    }
}
