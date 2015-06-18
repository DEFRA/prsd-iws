namespace EA.Iws.Requests.Notification
{
    using System;

    public class RecoveryPercentageData
    {
        public Guid NotificationId { get; set; }

        public bool? IsProvidedByImporter { get; set; }

        public decimal? PercentageRecoverable { get; set; }

        public string MethodOfDisposal { get; set; }
    }
}
