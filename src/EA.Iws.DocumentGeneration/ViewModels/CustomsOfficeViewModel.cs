﻿namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.TransportRoute;
    using System;

    internal class CustomsOfficeViewModel
    {
        public CustomsOfficeViewModel(TransportRoute transportRoute)
        {
            EntryTitle = transportRoute.EntryCustomsOffice != null ? "Entry customs office:" : string.Empty;
            ExitTitle = transportRoute.ExitCustomsOffice != null ? "Exit customs office:" : string.Empty;
            EntryName = transportRoute.EntryCustomsOffice != null
                ? transportRoute.EntryCustomsOffice.Name
                : string.Empty;
            ExitName = transportRoute.ExitCustomsOffice != null ? transportRoute.ExitCustomsOffice.Name : string.Empty;
            EntryAddress = transportRoute.EntryCustomsOffice != null
                ? transportRoute.EntryCustomsOffice.Address
                : string.Empty;
            ExitAddress = transportRoute.ExitCustomsOffice != null
                ? transportRoute.ExitCustomsOffice.Address
                : string.Empty;
            EntryCountry = transportRoute.EntryCustomsOffice != null
                ? transportRoute.EntryCustomsOffice.Country.Name
                : string.Empty;
            ExitCountry = transportRoute.ExitCustomsOffice != null
                ? transportRoute.ExitCustomsOffice.Country.Name
                : string.Empty;
            EntryAnnexMessage = string.Empty;
            ExitAnnexMessage = string.Empty;
            SetIsAnnexNeeded(transportRoute);
            CustomsOfficeData = string.Empty;
        }

        public string EntryTitle { get; private set; }

        public string ExitTitle { get; private set; }

        public string EntryName { get; private set; }

        public string ExitName { get; private set; }

        public string EntryAddress { get; private set; }

        public string ExitAddress { get; private set; }

        public string EntryCountry { get; private set; }

        public string ExitCountry { get; private set; }

        public string EntryAnnexMessage { get; private set; }

        public string ExitAnnexMessage { get; private set; }

        public bool IsAnnexNeeded { get; private set; }

        public string CustomsOfficeData { get; set; }

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

        public void SetDisplayCustomsOfficeDetails(bool isNorthenIrelandCompetentAuthority)
        {
            if (isNorthenIrelandCompetentAuthority)
            {
                CustomsOfficeData = ExitTitle + Environment.NewLine +
                                ExitName + Environment.NewLine +
                                ExitAddress + Environment.NewLine +
                                ExitCountry + Environment.NewLine + " " + Environment.NewLine +
                                EntryTitle + Environment.NewLine +
                                EntryName + Environment.NewLine +
                                EntryAddress + Environment.NewLine +
                                EntryCountry + Environment.NewLine;
            }
            else
            {
                CustomsOfficeData = EntryTitle + Environment.NewLine +
                                EntryName + Environment.NewLine +
                                EntryAddress + Environment.NewLine +
                                EntryCountry + Environment.NewLine + " " + Environment.NewLine +
                                ExitTitle + Environment.NewLine +
                                ExitName + Environment.NewLine +
                                ExitAddress + Environment.NewLine +
                                ExitCountry + Environment.NewLine;
            }
        }

        private void SetIsAnnexNeeded(TransportRoute transportRoute)
        {
            IsAnnexNeeded = transportRoute.EntryCustomsOffice != null || transportRoute.ExitCustomsOffice != null;
        }
    }
}