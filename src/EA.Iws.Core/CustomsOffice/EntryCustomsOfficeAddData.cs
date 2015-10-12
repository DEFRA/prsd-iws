namespace EA.Iws.Core.CustomsOffice
{
    using System.Collections.Generic;
    using Shared;

    public class EntryCustomsOfficeAddData
    {
        public CustomsOffices CustomsOfficesRequired { get; set; }

        public CustomsOfficeData CustomsOfficeData { get; set; }

        public IList<CountryData> Countries { get; set; }
    }
}