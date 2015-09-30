namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core.Notification;
    using Core.Shared;
    using Core.TechnologyEmployed;
    using Requests.Notification.Overview;

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

        public RecoveryOperationViewModel(RecoveryOperation recoveryOperationInfo)
        {
            NotificationId = recoveryOperationInfo.NotificationId;
            NotificationType = recoveryOperationInfo.NotificationType;
            IsPreconsentStatusChosen = recoveryOperationInfo.IsPreconsentStatusChosen;
            AreOperationCodesChosen = recoveryOperationInfo.AreOperationCodesChosen;
            IsTechnologyEmployedCompleted = recoveryOperationInfo.IsTechnologyEmployedCompleted;
            IsReasonForExportCompleted = recoveryOperationInfo.IsReasonForExportCompleted;
            PreconstedAnswer = recoveryOperationInfo.PreconstedAnswer;
            OperationCodes = recoveryOperationInfo.OperationCodes.OrderBy(c => c.Value).Select(c => c.Code).ToList();
            TechnologyEmployed = recoveryOperationInfo.TechnologyEmployed;
            ReasonForExport = recoveryOperationInfo.ReasonForExport;
        }
    }
}