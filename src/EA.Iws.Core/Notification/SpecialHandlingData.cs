namespace EA.Iws.Core.Notification
{
    using System;

    public class SpecialHandlingData
    {
        public Guid NotificationId { get; set; }

        public bool? HasSpecialHandlingRequirements { get; set; }

        public string SpecialHandlingDetails { get; set; }
    }
}