namespace EA.Iws.Domain.TransportRoute
{
    using Prsd.Core.Domain;

    public class AllTransitStatesInEUEvent : IEvent
    {
        public AllTransitStatesInEUEvent(TransportRoute transportRoute)
        {
            TransportRoute = transportRoute;
        }

        public TransportRoute TransportRoute { get; private set; }
    }
}