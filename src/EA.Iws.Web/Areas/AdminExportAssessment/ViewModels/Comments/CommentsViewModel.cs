namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Comments
{
    using System;
    using System.Collections.Generic;
    using Core.Admin;
    using Core.InternalComments;

    public class CommentsViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationShipmentsCommentsType Type { get; set; }

        public List<InternalComment> Comments { get; set; }

        public CommentsViewModel()
        {
            this.Comments = new List<InternalComment>();
            this.Type = NotificationShipmentsCommentsType.Notification;
        }
    }
}