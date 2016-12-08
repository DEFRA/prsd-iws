namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;

    [DisplayName("Type of waste")]
    public class ChemicalComposition : IDraftEntity
    {
        public ChemicalComposition(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        internal ChemicalComposition()
        {
        }

        public Core.WasteType.ChemicalComposition? Composition { get; set; }

        public Guid ImportNotificationId { get; private set; }
    }
}