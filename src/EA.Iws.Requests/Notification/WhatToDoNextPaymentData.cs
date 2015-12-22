namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Core.Shared;

    public class WhatToDoNextPaymentData
    {
        public decimal Charge { get; set; }

        public decimal AmountPaid { get; set; }

        public string NotificationNumber { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public UnitedKingdomCompetentAuthorityData UnitedKingdomCompetentAuthorityData { get; set; }

        public NotificationType NotificationType { get; set; }

        public Guid Id { get; set; }
    }
}
