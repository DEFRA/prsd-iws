namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    [AutoRegister]
    public class NotificationApplicationFactory
    {
        private const int EaNumberSystemStart = 5000;
        private const int SepaNumberSystemStart = 500;
        private const int NieaNumberSystemStart = 1000;
        private const int NrwNumberSystemStart = 100;

        private readonly INotificationNumberGenerator numberGenerator;
        private readonly IUserContext userContext;

        public NotificationApplicationFactory(IUserContext userContext, INotificationNumberGenerator numberGenerator)
        {
            this.userContext = userContext;
            this.numberGenerator = numberGenerator;
        }

        public Task<NotificationApplication> CreateLegacy(NotificationType notificationType,
            UKCompetentAuthority competentAuthority, int number)
        {
            Guard.ArgumentNotZeroOrNegative(() => number, number);

            if (!IsNumberValid(competentAuthority, number))
            {
                throw new ArgumentOutOfRangeException("number",
                    string.Format("{0} is out of range for a notification number for {1}", number, competentAuthority));
            }

            var notification = new NotificationApplication(userContext.UserId, notificationType, competentAuthority,
                number);
            return Task.FromResult(notification);
        }

        public async Task<NotificationApplication> CreateNew(NotificationType notificationType,
            UKCompetentAuthority competentAuthority)
        {
            var nextNotificationNumber = await numberGenerator.GetNextNotificationNumber(competentAuthority);
            var notification = new NotificationApplication(userContext.UserId, notificationType, competentAuthority,
                nextNotificationNumber);
            return notification;
        }

        private static bool IsNumberValid(UKCompetentAuthority competentAuthority, int number)
        {
            return (competentAuthority == UKCompetentAuthority.England && number < EaNumberSystemStart)
                || (competentAuthority == UKCompetentAuthority.Scotland && number < SepaNumberSystemStart)
                || (competentAuthority == UKCompetentAuthority.NorthernIreland && number < NieaNumberSystemStart)
                || (competentAuthority == UKCompetentAuthority.Wales && number < NrwNumberSystemStart);
        }
    }
}