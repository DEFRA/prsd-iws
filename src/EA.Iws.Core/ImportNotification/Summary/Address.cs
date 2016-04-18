namespace EA.Iws.Core.ImportNotification.Summary
{
    public class Address
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string TownOrCity { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(AddressLine1) &&
                   string.IsNullOrWhiteSpace(AddressLine2) &&
                   string.IsNullOrWhiteSpace(TownOrCity) &&
                   string.IsNullOrWhiteSpace(PostalCode) &&
                   string.IsNullOrWhiteSpace(Country);
        }
    }
}
