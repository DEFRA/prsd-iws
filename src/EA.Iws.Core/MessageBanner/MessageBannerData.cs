namespace EA.Iws.Core.MessageBanner
{
    using System;

    public class MessageBannerData
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
    }
}
