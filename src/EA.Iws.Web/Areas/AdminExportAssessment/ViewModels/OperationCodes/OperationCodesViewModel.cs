namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.OperationCodes
{
    using System.Collections.Generic;
    using System.Linq;
    using EA.Iws.Core.Notification.Overview;
    using EA.Iws.Core.OperationCodes;
    using EA.Iws.Core.Shared;
    using EA.Iws.Web.ViewModels.Shared;
    using EA.Prsd.Core.Helpers;
    using Web.Areas.AdminExportAssessment.Views.OperationCodes;

    public class OperationCodesViewModel
    {
        public string Title
        {
            get
            {
                return EditResources.Title.Replace("{type}", this.NotificationType.ToString());
            }
        }
        public string Header
        {
            get
            {
                return EditResources.Header.Replace("{type}", this.NotificationType.ToString());
            }
        }

        public NotificationType NotificationType { get; set; }

        public IList<KeyValuePairViewModel<OperationCode, bool>> PossibleCodes { get; set; }

        public IList<string> DisplayValues
        {
            get
            {
                return PossibleCodes.Select(c => string.Format("{0} - {1}",
                        EnumHelper.GetDisplayName(c.Key), EnumHelper.GetDescription(c.Key))).ToList();
            }
        }

        public IList<OperationCode> SelectedValues
        {
            get
            {
                if (PossibleCodes == null)
                {
                    return new List<OperationCode>(0);
                }

                return PossibleCodes.Where(c => c.Value).Select(c => c.Key).ToList();
            }
        }

        public OperationCodesViewModel()
        {
        }

        public OperationCodesViewModel(NotificationType type, IEnumerable<OperationCode> selectedCodes)
        {
            this.NotificationType = type;
            PossibleCodes = OperationCodeMetadata.GetCodesForOperation(type)
                .Select(c => new KeyValuePairViewModel<OperationCode, bool>(c, selectedCodes.Contains(c)))
                .ToList();
        }
    }
}