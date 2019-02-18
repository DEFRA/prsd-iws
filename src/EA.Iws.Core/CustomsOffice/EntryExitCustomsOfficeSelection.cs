namespace EA.Iws.Core.CustomsOffice
{
    public class EntryExitCustomsOfficeSelectionData
    {
        public bool? Entry { get; set; }

        public bool? Exit { get; set; }

        public EntryExitCustomsOfficeSelectionData(bool? entry, bool? exit)
        {
            this.Entry = entry;
            this.Exit = exit;
        }
    }
}
