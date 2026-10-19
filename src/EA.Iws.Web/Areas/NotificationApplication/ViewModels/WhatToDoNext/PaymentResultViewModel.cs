namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WhatToDoNext
{
    using System;

    public class PaymentResultViewModel
    {
        public Guid NotificationId { get; set; }

        public string PaymentReference { get; set; }

        public decimal AmountPaid { get; set; }
    }
}