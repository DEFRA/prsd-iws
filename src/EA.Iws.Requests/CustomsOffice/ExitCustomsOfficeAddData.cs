namespace EA.Iws.Requests.CustomsOffice
{
    using System.Collections.Generic;
    using Core.CustomsOffice;
    using Core.Shared;

    public class ExitCustomsOfficeAddData
    {
        public CustomsOffices CustomsOffices { get; set; }

        public CustomsOfficeData CustomsOfficeData { get; set; }

        public IList<CountryData> Countries { get; set; }
    }
}
