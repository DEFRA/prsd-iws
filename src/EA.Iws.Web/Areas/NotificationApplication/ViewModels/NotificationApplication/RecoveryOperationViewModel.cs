namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.Shared;
    using Core.TechnologyEmployed;
    using Prsd.Core.Helpers;

    public class RecoveryOperationViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsPreconsentStatusChosen { get; set; }
        public bool AreOperationCodesChosen { get; set; }
        public bool IsTechnologyEmployedCompleted { get; set; }
        public bool IsReasonForExportCompleted { get; set; }
        public string PreconstedAnswer { get; set; }
        public List<string> OperationCodes { get; set; }
        public TechnologyEmployedData TechnologyEmployed { get; set; }
        public string ReasonForExport { get; set; }

        public RecoveryOperationViewModel()
        {
        }

        public RecoveryOperationViewModel(RecoveryOperation recoveryOperationInfo, NotificationApplicationCompletionProgress progress)
        {
            NotificationId = recoveryOperationInfo.NotificationId;
            NotificationType = recoveryOperationInfo.NotificationType;
            IsPreconsentStatusChosen = progress.HasPreconsentedInformation;
            AreOperationCodesChosen = progress.HasOperationCodes;
            IsTechnologyEmployedCompleted = progress.HasTechnologyEmployed;
            IsReasonForExportCompleted = progress.HasReasonForExport;
            PreconstedAnswer = recoveryOperationInfo.PreconstedAnswer;
            OperationCodes = recoveryOperationInfo.OperationCodes.OrderBy(c => c).Select(EnumHelper.GetDisplayName).ToList();
            TechnologyEmployed = recoveryOperationInfo.TechnologyEmployed;
            ReasonForExport = recoveryOperationInfo.ReasonForExport;
        }
    }
}