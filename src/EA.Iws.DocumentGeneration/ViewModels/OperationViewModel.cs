namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System.Collections.Generic;
    using System.Text;
    using Domain.Notification;

    internal class OperationViewModel
    {
        public OperationViewModel(OperationViewModel model, int annexNumber)
        {
            IsAnnexProvided = model.IsAnnexProvided;
            ReasonForExport = model.ReasonForExport;
            OperationCodes = model.OperationCodes;
            TechnologyEmployed = "See annex. " + annexNumber;
        }

        public OperationViewModel(NotificationApplication notification)
        {
            IsAnnexProvided = notification.TechnologyEmployed.AnnexProvided;
            ReasonForExport = notification.ReasonForExport;
            TechnologyEmployed = IsAnnexProvided ? "See annex. 3" : notification.TechnologyEmployed.Details;

            SetOperationCodes(notification.OperationInfos);
        }

        public string OperationCodes { get; private set; }

        public bool IsAnnexProvided { get; private set; }

        public string TechnologyEmployed { get; private set; }

        public string ReasonForExport { get; private set; }

        private void SetOperationCodes(IEnumerable<OperationInfo> operationInfos)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var operationInfo in operationInfos)
            {
                sb.Append(operationInfo.OperationCode.DisplayName);
                sb.Append(",");
            }

            OperationCodes = sb.ToString().Substring(0, sb.ToString().Length - 1);
        }
    }
}