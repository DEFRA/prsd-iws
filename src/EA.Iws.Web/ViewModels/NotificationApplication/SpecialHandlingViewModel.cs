namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using System;

    public class SpecialHandlingViewModel
    {
        public bool IsSpecialHandling { get; set; }

        public Guid NotificationId { get; set; }

        public string SpecialHandlingDetails { get; set; }
    }
}