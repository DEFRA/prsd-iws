namespace EA.Iws.Domain.Movement.BulkUpload
{
    using System;
    using Prsd.Core.Domain;

    public class DraftBulkUpload : Entity
    {
        public Guid NotificationId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string FileName { get; set; }

        public DraftBulkUpload(Guid notificationId, DateTime createDate, string createdBy, string fileName)
        {
            NotificationId = notificationId;
            CreatedDate = createDate;
            CreatedBy = createdBy;
            FileName = fileName;
        }
    }
}
