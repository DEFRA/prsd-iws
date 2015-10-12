namespace EA.Iws.Core.CustomsOffice
{
    using System.Collections.Generic;
    using Shared;

    public class ExitCustomsOfficeAddData
    {
        public CustomsOffices CustomsOffices { get; set; }

        public CustomsOfficeData CustomsOfficeData { get; set; }

        public IList<CountryData> Countries { get; set; }
    }
}
