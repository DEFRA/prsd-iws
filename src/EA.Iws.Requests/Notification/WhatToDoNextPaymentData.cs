namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Core.NotificationAssessment;
    using Core.Shared;

    public class WhatToDoNextPaymentData
    {
        public decimal Charge { get; set; }

        public decimal AmountPaid { get; set; }

        public string NotificationNumber { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public UnitedKingdomCompetentAuthorityData UnitedKingdomCompetentAuthorityData { get; set; }

        public NotificationType NotificationType { get; set; }

        public Guid Id { get; set; }

        public NotificationStatus Status { get; set; }
    }
}
