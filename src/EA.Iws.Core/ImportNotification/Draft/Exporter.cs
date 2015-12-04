namespace EA.Iws.Core.ImportNotification.Draft
{
    using System.ComponentModel;

    [DisplayName("Silly exporter")]
    public class Exporter
    {
        public Address Address { get; set; }

        public string BusinessName { get; set; }

        public Contact Contact { get; set; }
    }
}