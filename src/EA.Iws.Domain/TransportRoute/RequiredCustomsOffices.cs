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

            var isStartPointEU = transportRoute.StateOfExport.Country.IsEuropeanUnionMember;
            var isEndPointEU = transportRoute.StateOfImport.Country.IsEuropeanUnionMember;
            var areAllTransitStatesEU = isStartPointEU && isEndPointEU;

            if (transportRoute.TransitStates != null)
            {
                areAllTransitStatesEU = transportRoute.TransitStates.All(ts => ts.Country.IsEuropeanUnionMember);
            }

            if (isEndPointEU)
            {
                return areAllTransitStatesEU ? CustomsOffices.None : CustomsOffices.EntryAndExit;
            }
            return CustomsOffices.Exit;
        }
    }
}