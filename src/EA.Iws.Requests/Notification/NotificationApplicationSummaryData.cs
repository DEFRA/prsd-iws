namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.NotificationAssessment;

    public class NotificationApplicationSummaryData
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationStatus Status { get; set; }

        public DateTimeOffset StatusDate { get; set; }

        public string Exporter { get; set; }

        public string Producer { get; set; }

        public string Importer { get; set; }

        public string AccessLevel { get; set; }

        public DateTime? ConsentedFrom { get; set; }

        public string ConsentedFromText 
        { 
            get 
            {
                if (ConsentedFrom == null)
                {
                    return null;
                }
                else
                {
                    return "From: " + ConsentedFrom?.ToString("dd/MM/yyyy");
                }
            } 
        }

        public DateTime? ConsentedTo { get; set; }

        public string ConsentedToText
        {
            get
            {
                if (ConsentedTo == null)
                {
                    return null;
                }
                else
                {
                    return "To: " + ConsentedTo?.ToString("dd/MM/yyyy");
                }
            }
        }
    }
}
