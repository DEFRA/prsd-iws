namespace EA.Iws.Requests.Notification
{
    using System;

    [Serializable]
    public class NotificationUpdateHistory
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; }
        
        public DateTimeOffset DateAdded { get; set; }
        
        public string InformationChange { get; set; }

        public string TypeOfChange { get; set; }
    }
}
