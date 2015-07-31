namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Domain.TransportRoute;

    internal class TransitStateViewModel
    {
        public string LeftCountry { get; private set; }
        public string LeftCa { get; private set; }
        public string LeftEntry { get; private set; }
        public string LeftExit { get; private set; }

        public string MiddleCountry { get; private set; }
        public string MiddleCa { get; private set; }
        public string MiddleEntry { get; private set; }
        public string MiddleExit { get; private set; }

        public string RightCountry { get; private set; }
        public string RightCa { get; private set; }
        public string RightEntry { get; private set; }
        public string RightExit { get; private set; }

        public string AnnexMessage { get; private set; }

        public bool IsAnnexNeeded { get; private set; }

        public List<TransitStateDetail> TransitStateDetails { get; private set; }

        public TransitStateViewModel(List<TransitState> transitStates)
        {
            SetAllPropertiesToEmptyString();

            SetIsAnnexNeeded(transitStates);

            if (transitStates.Count == 1)
            {
                SetPropertiesForOneTransitState(transitStates);
            }

            if (transitStates.Count == 2)
            {
                SetPropertiesForTwoTransitStates(transitStates);
            }

            if (transitStates.Count == 3)
            {
                SetPropertiesForThreeTransitStates(transitStates);
            }

            if (IsAnnexNeeded)
            {
                SetAllPropertiesForMoreThanThreeStates();
                SetupTransitStateDetails(transitStates);
            }
        }

        public TransitStateViewModel()
        {
        }

        private void SetupTransitStateDetails(List<TransitState> transitStates)
        {
            foreach (var state in transitStates)
            {
                var n = new TransitStateDetail
                {
                    Country = state.Country.Name,
                    Ca = state.CompetentAuthority.Code,
                    Entry = state.EntryPoint.Name,
                    Exit = state.ExitPoint.Name
                };

                TransitStateDetails.Add(n);
            }
        }

        private void SetPropertiesForOneTransitState(List<TransitState> transitStates)
        {
            MiddleCountry = transitStates[0].Country.Name;
            MiddleCa = transitStates[0].CompetentAuthority.Code;
            MiddleEntry = transitStates[0].EntryPoint.Name;
            MiddleExit = transitStates[0].ExitPoint.Name;
        }

        private void SetPropertiesForTwoTransitStates(List<TransitState> transitStates)
        {
            LeftCountry = transitStates[0].Country.Name;
            LeftCa = transitStates[0].CompetentAuthority.Code;
            LeftEntry = transitStates[0].EntryPoint.Name;
            LeftExit = transitStates[0].ExitPoint.Name;

            MiddleCountry = string.Empty;
            MiddleCa = string.Empty;
            MiddleEntry = string.Empty;
            MiddleExit = string.Empty;

            RightCountry = transitStates[1].Country.Name;
            RightCa = transitStates[1].CompetentAuthority.Code;
            RightEntry = transitStates[1].EntryPoint.Name;
            RightExit = transitStates[1].ExitPoint.Name;

            AnnexMessage = string.Empty;
        }

        private void SetPropertiesForThreeTransitStates(List<TransitState> transitStates)
        {
            LeftCountry = transitStates[0].Country.Name;
            LeftCa = transitStates[0].CompetentAuthority.Code;
            LeftEntry = transitStates[0].EntryPoint.Name;
            LeftExit = transitStates[0].ExitPoint.Name;

            MiddleCountry = transitStates[1].Country.Name;
            MiddleCa = transitStates[1].CompetentAuthority.Code;
            MiddleEntry = transitStates[1].EntryPoint.Name;
            MiddleExit = transitStates[1].ExitPoint.Name;

            RightCountry = transitStates[2].Country.Name;
            RightCa = transitStates[2].CompetentAuthority.Code;
            RightEntry = transitStates[2].EntryPoint.Name;
            RightExit = transitStates[2].ExitPoint.Name;

            AnnexMessage = string.Empty;
        }

        private void SetAllPropertiesForMoreThanThreeStates()
        {
            LeftCountry = string.Empty;
            LeftCa = string.Empty;
            LeftEntry = string.Empty;
            LeftExit = string.Empty;

            MiddleCountry = string.Empty;
            MiddleCa = string.Empty;
            MiddleEntry = string.Empty;
            MiddleExit = string.Empty;

            RightCountry = string.Empty;
            RightCa = string.Empty;
            RightEntry = string.Empty;
            RightExit = string.Empty;

            AnnexMessage = "See Annex";
        }

        private void SetAllPropertiesToEmptyString()
        {
            LeftCountry = string.Empty;
            LeftCa = string.Empty;
            LeftEntry = string.Empty; 
            LeftExit = string.Empty;
            
            MiddleCountry = string.Empty;
            MiddleCa = string.Empty;
            MiddleEntry = string.Empty;
            MiddleExit = string.Empty;
            
            RightCountry = string.Empty;
            RightCa = string.Empty;
            RightEntry = string.Empty;
            RightExit = string.Empty;

            AnnexMessage = string.Empty;

            TransitStateDetails = new List<TransitStateDetail>();
        }

        private void SetIsAnnexNeeded(List<TransitState> transitStates)
        {
            IsAnnexNeeded = transitStates.Count() > 3 || AreAnyOfTheTransitStateEntryOrExitNamesGreaterThanTwelveCharacters(transitStates);
        }

        private bool AreAnyOfTheTransitStateEntryOrExitNamesGreaterThanTwelveCharacters(List<TransitState> transitStates)
        {
            var result = false;

            foreach (var state in transitStates)
            {
                if (state.EntryPoint.Name.Length > 12 || state.ExitPoint.Name.Length > 12)
                {
                    result = true;
                }
            }

            return result;
        }

        public void SetAnnexMessage(int annexNumber)
        {
            AnnexMessage = "See Annex " + annexNumber;
        }
    }
}
