namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Comments
{
    using EA.Iws.Core.InternalComments;
    using System;
    using AdminExportAssessment.Views.Comments;
    using EA.Iws.Core.Admin;

    public class DeleteCommentViewModel
    {
        public Guid NotificationId { get; set; }
        public Guid CommentId { get; set; }
        public InternalComment Comment { get; set; }

        public NotificationShipmentsCommentsType Type {get; set; }
        public string ConfirmationText
        {
            get
            {
                string end = Comment.ShipmentNumber == 0 ? string.Empty : string.Format(" about shipment number {0}", Comment.ShipmentNumber);
                return string.Format("{0} {1}{2}.", IndexResources.DeleteText, Comment.Username, end);
            }
        }
    }
}