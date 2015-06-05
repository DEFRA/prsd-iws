namespace EA.Iws.Domain.TransportRoute
{
    using Prsd.Core.Domain;

    public class StateOfImport : Entity, IState, IEnter
    {
        public Country Country { get; protected set; }

        public CompetentAuthority CompetentAuthority { get; protected set; }

        public EntryOrExitPoint EntryPoint { get; protected set; }

        protected StateOfImport()
        {
        }
    }
}