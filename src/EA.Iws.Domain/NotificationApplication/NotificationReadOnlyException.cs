namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotificationReadOnlyException : Exception
    {
        public Guid NotificationId { get; private set; }

        public NotificationReadOnlyException(Guid notificationId) : base(GetMessage(notificationId))
        {
            NotificationId = notificationId;
        }

        public NotificationReadOnlyException(Guid notificationId, Exception innerException) : base(GetMessage(notificationId), innerException)
        {
            NotificationId = notificationId;
        }

        protected NotificationReadOnlyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            NotificationId = (Guid)info.GetValue("NotificationId", typeof(Guid));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("NotificationId", NotificationId);
        }

        private static string GetMessage(Guid notificationId)
        {
            return string.Format("Notification {0} is read-only and cannot be edited.", notificationId);
        }
    }
}