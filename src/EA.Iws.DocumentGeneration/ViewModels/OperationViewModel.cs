namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Domain.Notification;

    internal class OperationViewModel
    {
        public OperationViewModel(OperationViewModel model, int annexNumber)
        {
            IsAnnexProvided = model.IsAnnexProvided;
            ReasonForExport = model.ReasonForExport;
            OperationCodes = model.OperationCodes;
            TechnologyEmployedDetails = model.TechnologyEmployedDetails;
            FurtherDetails = model.FurtherDetails ?? string.Empty;
            AnnexIfFurtherDetailsProvided = "See Annex " + annexNumber;
            AnnexProvided = model.IsAnnexProvided ? "See Annex" : string.Empty;
        }

        public OperationViewModel(NotificationApplication notification)
        {
            IsAnnexProvided = notification.TechnologyEmployed.AnnexProvided;
            ReasonForExport = notification.ReasonForExport;
            TechnologyEmployedDetails = notification.TechnologyEmployed.Details;
            AreFurtherDetailsProvided = notification.TechnologyEmployed.FurtherDetails != null;
            FurtherDetails = notification.TechnologyEmployed.FurtherDetails ?? string.Empty;
            SetOperationCodes(notification.OperationInfos);
            AnnexIfFurtherDetailsProvided = string.Empty;
            AnnexProvided = notification.TechnologyEmployed.AnnexProvided ? "See Annex" : string.Empty;
        }

        public string OperationCodes { get; private set; }

        public bool IsAnnexProvided { get; private set; }

        public bool AreFurtherDetailsProvided { get; private set; }

        public string TechnologyEmployedDetails { get; private set; }

        public string FurtherDetails { get; private set; }

        public string AnnexIfFurtherDetailsProvided { get; private set; }

        public string AnnexProvided { get; private set; }

        public string ReasonForExport { get; private set; }

        private void SetOperationCodes(IEnumerable<OperationInfo> operationInfos)
        {
            var orderedOperationInfos = operationInfos.OrderBy(c => c.OperationCode.Value);

            StringBuilder sb = new StringBuilder();
            foreach (var operationInfo in orderedOperationInfos)
            {
                sb.Append(operationInfo.OperationCode.DisplayName);
                sb.Append(", ");
            }

            OperationCodes = sb.ToString().Substring(0, sb.ToString().Length - 2);
        }
    }
}