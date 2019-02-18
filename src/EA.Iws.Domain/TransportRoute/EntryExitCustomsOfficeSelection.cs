namespace EA.Iws.Domain.TransportRoute
{
    using Prsd.Core.Domain;

    public class EntryExitCustomsOfficeSelection : Entity
    {
        public bool? Entry { get; private set; }

        public bool? Exit { get; private set; }

        protected EntryExitCustomsOfficeSelection()
        {
        }

        public EntryExitCustomsOfficeSelection(bool? entry, bool? exit)
        {
            this.Entry = entry;
            this.Exit = exit;
        }
    }
}
