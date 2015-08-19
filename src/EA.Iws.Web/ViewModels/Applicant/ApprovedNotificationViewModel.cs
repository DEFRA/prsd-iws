namespace EA.Iws.Web.ViewModels.Applicant
{
    using System;
    using System.Collections.Generic;
    using Core.Shared;
    using Shared;

    public class ApprovedNotificationViewModel
    {
        private const string PrintNotification = "Print notification document";
        private const string ViewNotification = "View notification";
        private const string GeneratePrenotification = "Generate prenotification";
        private const string RecordCertificateOfReceipt = "Record certificate of receipt";
        private const string RecordCertificateOfDisposal = "Record certificate of disposal";
        private const string RecordCertificateOfRecovery = "Record certificate of recovery";

        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public StringIntRadioButtons UserChoices { get; set; }

        public ApprovedNotificationViewModel()
        {
        }

        public ApprovedNotificationViewModel(NotificationType notificationType)
        {
            NotificationType = notificationType;

            var choices = new List<KeyValuePair<string, int>>();
            choices.Add(new KeyValuePair<string, int>(ViewNotification, 1));
            choices.Add(new KeyValuePair<string, int>(PrintNotification, 2));
            choices.Add(new KeyValuePair<string, int>(GeneratePrenotification, 3));
            choices.Add(new KeyValuePair<string, int>(RecordCertificateOfReceipt, 4));
            choices.Add(notificationType == NotificationType.Disposal
                ? new KeyValuePair<string, int>(RecordCertificateOfDisposal, 5)
                : new KeyValuePair<string, int>(RecordCertificateOfRecovery, 6));

            UserChoices = new StringIntRadioButtons(choices);
        }
    }
}