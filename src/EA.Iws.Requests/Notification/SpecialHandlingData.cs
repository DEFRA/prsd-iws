namespace EA.Iws.Requests.Notification
{
    using System;

    public class SpecialHandlingData
    {
        public Guid NotificationId { get; set; }

        public bool? HasSpecialHandlingRequirements { get; set; }

        public string SpecialHandlingDetails { get; set; }
    }
}