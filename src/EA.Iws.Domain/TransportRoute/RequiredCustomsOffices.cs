namespace EA.Iws.Domain.TransportRoute
{
    using System.Linq;
    using Core.CustomsOffice;

    public class RequiredCustomsOffices
    {
        public CustomsOffices GetForTransportRoute(TransportRoute transportRoute)
        {
            return CustomsOffices.EntryAndExit;
        }
    }
}