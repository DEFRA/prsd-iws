namespace EA.Iws.Domain.TransportRoute
{
    using Prsd.Core.Domain;

    public class EntryExitCustomsSelection : Entity
    {
        public bool Entry { get; private set; }

        public bool Exit { get; private set; }

        protected EntryExitCustomsSelection()
        {
        }

        public EntryExitCustomsSelection(bool entry, bool exit)
        {
            this.Entry = entry;
            this.Exit = exit;
        }
    }
}
