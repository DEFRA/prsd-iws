namespace EA.Iws.Core.ImportNotification.Summary
{
    public class Exporter
    {
        public Address Address { get; set; }

        public string Name { get; set; }

        public Contact Contact { get; set; }

        public bool IsEmpty()
        {
            return Address.IsEmpty()
                   && string.IsNullOrWhiteSpace(Name)
                   && Contact.IsEmpty();
        }
    }
}
