namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public string WasteGenerationProcess { get; private set; }

        public bool? IsWasteGenerationProcessAttached { get; private set; }

        public void AddWasteGenerationProcess(string process, bool isDocumentAttached)
        {
            if (!isDocumentAttached && string.IsNullOrWhiteSpace(process))
            {
                throw new InvalidOperationException(
                    string.Format("Waste generation process is required if a document is not attached. {0}", Id));
            }
            WasteGenerationProcess = process;
            IsWasteGenerationProcessAttached = isDocumentAttached;
        }
    }
}