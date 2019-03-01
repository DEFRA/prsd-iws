namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditComments)]
    public class AddImportNotificationComment : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public Guid UserId { get; private set; }

        public string Comment { get; private set; }

        public int ShipmentNumber { get; private set; }

        public DateTime DateAdded { get; private set; }

        public AddImportNotificationComment(Guid importNotificationId, Guid userId, string comment, int shipmentNumber, DateTime dateAdded)
        {
            this.ImportNotificationId = importNotificationId;
            this.UserId = userId;
            this.Comment = comment;
            this.ShipmentNumber = shipmentNumber;
            this.DateAdded = dateAdded;
        }
    }
}
