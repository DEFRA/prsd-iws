namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.NotificationApplication;
    using Formatters;

    internal class OperationViewModel
    {
        private string operationCodes = string.Empty;
        private readonly string technologyEmployedDetails = string.Empty;
        private readonly string furtherDetails = string.Empty;
        private readonly string annexProvided = string.Empty;
        private readonly string reasonForExport = string.Empty;

        public OperationViewModel(OperationViewModel model, int annexNumber)
        {
            IsAnnexProvided = model.IsAnnexProvided;
            reasonForExport = model.ReasonForExport;
            OperationCodes = model.OperationCodes;
            technologyEmployedDetails = model.TechnologyEmployedDetails;
            furtherDetails = model.FurtherDetails ?? string.Empty;
            annexProvided = GetAnnexProvidedText(model, annexNumber);
        }

        public OperationViewModel(NotificationApplication notification, TechnologyEmployed technologyEmployed, OperationInfoFormatter formatter)
        {
            if (notification == null)
            {
                return;
            }

            reasonForExport = notification.ReasonForExport ?? string.Empty;
            OperationCodes = formatter.OperationInfosToCommaDelimitedList(notification.OperationInfos);

            if (technologyEmployed != null)
            {
                IsAnnexProvided = technologyEmployed.AnnexProvided;
                technologyEmployedDetails = technologyEmployed.Details ?? string.Empty;
                furtherDetails = technologyEmployed.FurtherDetails ?? string.Empty;
            }
        }

        public string OperationCodes
        {
            get { return operationCodes; }
            private set { operationCodes = value; }
        }

        public bool IsAnnexProvided { get; private set; }

        public string TechnologyEmployedDetails
        {
            get { return technologyEmployedDetails; }
        }

        public string FurtherDetails
        {
            get { return furtherDetails; }
        }

        public string AnnexProvided
        {
            get { return annexProvided; }
        }

        public string ReasonForExport
        {
            get { return reasonForExport; }
        }

        private string GetAnnexProvidedText(OperationViewModel model, int annexNumber)
        {
            var text = string.Empty;

            if (model.IsAnnexProvided || !string.IsNullOrEmpty(model.FurtherDetails))
            {
                text = "See Annex " + annexNumber;
            }

            return text;
        }
    }
}