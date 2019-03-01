namespace EA.Iws.Core.InternalComments
{
    using System;

    public class InternalComment
    {
        public Guid NotificationId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public int ShipmentNumber { get; set; }
        public DateTime DateAdded { get; set; }

    }
}
