namespace EA.Iws.Requests.Notification
{
    using System;

    public class NotificationUpdateHistorySummaryData
    {
        //COULLM: Is this id needed? What is this the id of?
        public Guid Id { get; set; }

        //COULLM: Whose name?
        public string Name { get; set; }

        //COULLM: Which date?
        public string Date { get; set; }

        //COULLM: What time?
        public string Time { get; set; }

        //COULLM: Should the type be an enum? If so does it already exist?
        public string InformationChange { get; set; }

        //COULLM: Should the type be an enum? If so does it already exist? EA.Iws.Core/Notification/Audit/NotificationAuditType.cs
        public string TypeOfChange { get; set; }
    }
}
