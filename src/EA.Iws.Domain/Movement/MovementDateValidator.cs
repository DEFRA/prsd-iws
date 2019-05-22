﻿namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using NotificationConsent;
    using Prsd.Core;

    [AutoRegister]
    public class MovementDateValidator : IMovementDateValidator
    {
        private readonly INotificationConsentRepository consentRepository;

        public MovementDateValidator(INotificationConsentRepository consentRepository)
        {
            this.consentRepository = consentRepository;
        }

        public async Task EnsureDateValid(Guid notificationId, DateTime date)
        {
            var consent = await consentRepository.GetByNotificationId(notificationId);

            if (!consent.ConsentRange.Contains(date))
            {
                throw new MovementDateOutsideConsentPeriodException(string.Format(
                    "Can't set new movement date {0} since it is outside the consent range for this notification",
                    date));
            }

            if (date < SystemTime.UtcNow.Date)
            {
                throw new MovementDateOutOfRangeDateInPastException(string.Format(
                    "Can't set new movement date {0} since it is in the past",
                    date));
            }

            if (date > SystemTime.UtcNow.Date.AddDays(30))
            {
                throw new MovementDateOutOfRangeException(string.Format(
                    "Can't set new movement date {0} since it is more than 30 days in the future",
                    date));
            }
        }
    }
}