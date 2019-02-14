namespace EA.Iws.Core.CustomsOffice
{
    public class EntryExitCustomsSelectionData
    {
        public bool Entry { get; set; }

        public bool Exit { get; set; }

        public EntryExitCustomsSelectionData(bool entry, bool exit)
        {
            this.Entry = entry;
            this.Exit = exit;
        }
    }
}
