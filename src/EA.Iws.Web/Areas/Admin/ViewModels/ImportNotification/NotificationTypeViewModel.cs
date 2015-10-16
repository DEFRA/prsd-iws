namespace EA.Iws.Web.Areas.Admin.ViewModels.ImportNotification
{
    using System.Collections.Generic;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class NotificationTypeViewModel
    {
        public NotificationTypeViewModel()
        {
            NotificationTypeRadioButtons = new StringIntRadioButtons(new[]
            { 
                new KeyValuePair<string, int>(EnumHelper.GetDisplayName(NotificationType.Recovery), (int)NotificationType.Recovery),
                new KeyValuePair<string, int>(EnumHelper.GetDisplayName(NotificationType.Disposal), (int)NotificationType.Disposal)
            });
        }

        public StringIntRadioButtons NotificationTypeRadioButtons { get; set; }

        public string NotificationNumber { get; set; }
    }
}