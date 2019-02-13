namespace EA.Iws.Domain.TransportRoute
{
    using System.Linq;
    using Core.CustomsOffice;

    public class RequiredCustomsOffices
    {
        public CustomsOffices GetForTransportRoute(TransportRoute transportRoute)
        {
            if (transportRoute == null || transportRoute.StateOfExport == null || transportRoute.StateOfImport == null)
            {
                return CustomsOffices.TransitStatesNotSet;
            }
            return CustomsOffices.EntryAndExit;
        }
    }
}