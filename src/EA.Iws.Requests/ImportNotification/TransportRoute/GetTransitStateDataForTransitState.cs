namespace EA.Iws.Requests.ImportNotification.TransportRoute
{
    using System.Collections.Generic;
    using Core.ImportNotification.Draft;
    using Core.TransitState;
    using Prsd.Core.Mediator;

    public class GetTransitStateDataForTransitStates : IRequest<IList<TransitStateData>>
    {
        public IList<TransitState> TransitStates { get; private set; }

        public GetTransitStateDataForTransitStates(IList<TransitState> transitStates)
        {
            TransitStates = transitStates;
        }
    }
}
