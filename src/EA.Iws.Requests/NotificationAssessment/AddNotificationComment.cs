namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditInternalComments)]
    public class AddNotificationComment : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public Guid UserId { get; private set; }

        public string Comment { get; private set; }

        public int ShipmentNumber { get; private set; }

        public DateTime DateAdded { get; private set; }

        public AddNotificationComment(Guid notificationId, Guid userId, string comment, int shipmentNumber, DateTime dateAdded)
        {
            this.NotificationId = notificationId;
            this.UserId = userId;
            this.Comment = comment;
            this.ShipmentNumber = shipmentNumber;
            this.DateAdded = dateAdded;
        }
    }
}
