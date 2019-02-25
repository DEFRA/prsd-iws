namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Comments
{
    using System;
    using System.Collections.Generic;
    using Core.Admin;

    public class CommentsViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationShipmentsCommentsType Type { get; set; }

        public List<String> Comments { get; set; }

        public CommentsViewModel()
        {
            this.Comments = new List<string>();
            this.Type = NotificationShipmentsCommentsType.Notification;
        }
    }
}