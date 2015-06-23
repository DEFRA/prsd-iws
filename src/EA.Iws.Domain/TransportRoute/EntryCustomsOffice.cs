namespace EA.Iws.Domain.TransportRoute
{
    public class EntryCustomsOffice : CustomsOffice
    {
        protected EntryCustomsOffice()
        {
        }

        public EntryCustomsOffice(string name, string address, Country country) 
            : base(name, address, country)
        {
        }
    }
}
