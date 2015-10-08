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
        private const string ManageShipments = "Manage shipments";

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
            choices.Add(new KeyValuePair<string, int>(ManageShipments, 3));

            UserChoices = new StringIntRadioButtons(choices);
        }
    }
}