namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditComments)]
    public class DeleteImportNotificationComment : IRequest<bool>
    {
        public Guid CommentId { get; private set; }

        public DeleteImportNotificationComment(Guid commentId)
        {
            this.CommentId = commentId;
        }
    }
}
