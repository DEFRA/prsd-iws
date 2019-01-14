namespace EA.Iws.Requests.Notification
{
    using System;

    public class NotificationUpdateHistorySummaryData
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Date { get; set; }
        
        public string Time { get; set; }
        
        public string InformationChange { get; set; }
        
        public string TypeOfChange { get; set; }
    }
}
