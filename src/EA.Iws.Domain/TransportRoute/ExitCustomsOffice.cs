namespace EA.Iws.Domain.TransportRoute
{
    public class ExitCustomsOffice : CustomsOffice
    {
        protected ExitCustomsOffice()
        {
        }

        public ExitCustomsOffice(string name, string address, Country country)
            : base(name, address, country)
        {
        }
    }
}
