namespace EA.Iws.Core.Annexes
{
    using System;
    using Prsd.Core;

    public class AnnexUpload
    {
        public byte[] FileBytes { get; private set; }

        public string FileType { get; private set; }

        public Guid NotificationId { get; private set; }

        public AnnexUpload(byte[] fileBytes, string fileType, Guid notificationId)
        {
            Guard.ArgumentNotNull(() => fileBytes, fileBytes);
            Guard.ArgumentNotNullOrEmpty(() => fileType, fileType);
            Guard.ArgumentNotDefaultValue(() => notificationId, notificationId);

            FileBytes = fileBytes;
            FileType = fileType;
            NotificationId = notificationId;
        }
    }
}
