namespace EA.Iws.Requests.CustomsOffice
{
    using Core.CustomsOffice;

    public class CustomsOfficeCompletionStatus
    {
        public CustomsOffices CustomsOfficesRequired { get; set; }

        public CustomsOffices CustomsOfficesCompleted { get; set; }
    }
}
