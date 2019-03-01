namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditInternalComments)]
    public class DeleteNotificationComment : IRequest<bool>
    {
        public Guid CommentId { get; private set; }

        public DeleteNotificationComment(Guid commentId)
        {
            this.CommentId = commentId;
        }
    }
}
