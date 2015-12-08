namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;

    public class Address
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string PostalCode { get; set; }

        public string TownOrCity { get; set; }

        public Guid? CountryId { get; set; }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(AddressLine1)
                    && string.IsNullOrEmpty(AddressLine2)
                    && string.IsNullOrEmpty(PostalCode)
                    && string.IsNullOrEmpty(TownOrCity)
                    && !CountryId.HasValue;
            }
        }
    }
}
