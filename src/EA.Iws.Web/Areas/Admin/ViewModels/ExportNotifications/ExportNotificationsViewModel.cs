namespace EA.Iws.Web.Areas.Admin.ViewModels.ExportNotifications
{
    using System.Collections.Generic;
    using Core.Admin;

    public class ExportNotificationsViewModel
    {
        public IEnumerable<ExportNotificationOwnerDisplay> TableData { get; set; }
    }
}