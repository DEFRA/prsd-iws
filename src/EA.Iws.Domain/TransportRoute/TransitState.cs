namespace EA.Iws.Domain.TransportRoute
{
    using Prsd.Core.Domain;

    public class TransitState : Entity, IState, IExitPoint, IEntryPoint
    {
        public Country Country { get; protected set; }

        public CompetentAuthority CompetentAuthority { get; protected set; }

        public EntryOrExitPoint ExitPoint {get; protected set; }

        public EntryOrExitPoint EntryPoint { get; protected set; }

        protected TransitState()
        {
        }
    }
}