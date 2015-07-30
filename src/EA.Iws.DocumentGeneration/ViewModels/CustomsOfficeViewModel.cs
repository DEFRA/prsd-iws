namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System;
    using Domain.NotificationApplication;

    internal class CustomsOfficeViewModel
    {
        public string EntryName { get; private set; }
        public string ExitName { get; private set; }
        public string EntryAddress { get; private set; }
        public string ExitAddress { get; private set; }
        public string EntryAnnexMessage { get; private set; }
        public string ExitAnnexMessage { get; private set; }
        public bool IsAnnexNeeded { get; private set; }

        public CustomsOfficeViewModel(NotificationApplication notification)
        {
            EntryName = notification.EntryCustomsOffice != null ? notification.EntryCustomsOffice.Name : string.Empty;
            ExitName = notification.ExitCustomsOffice != null ? notification.ExitCustomsOffice.Name : string.Empty;
            EntryAddress = notification.EntryCustomsOffice != null ? notification.EntryCustomsOffice.Address : string.Empty;
            ExitAddress = notification.ExitCustomsOffice != null ? notification.ExitCustomsOffice.Address : string.Empty;
            EntryAnnexMessage = string.Empty;
            ExitAnnexMessage = string.Empty;
            SetIsAnnexNeeded(notification);
        }

        public void SetAnnexMessages(int annexNumber)
        {
            if (EntryName.Length > 0)
            {
                EntryAnnexMessage = "See Annex " + annexNumber;
            }

            if (ExitName.Length > 0)
            {
                ExitAnnexMessage = "See Annex " + annexNumber;
            }
        }

        private void SetIsAnnexNeeded(NotificationApplication notification)
        {
            IsAnnexNeeded = notification.EntryCustomsOffice != null || notification.ExitCustomsOffice != null;
        }
    }
}
