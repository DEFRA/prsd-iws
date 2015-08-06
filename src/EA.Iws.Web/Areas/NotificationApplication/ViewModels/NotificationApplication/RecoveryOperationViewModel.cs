namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core.Notification;
    using Core.Shared;
    using Core.TechnologyEmployed;
    using Requests.Notification;

    public class RecoveryOperationViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationApplicationCompletionProgress Progress { get; set; }
        public string PreconstedAnswer { get; set; }
        public List<string> OperationCodes { get; set; }
        public TechnologyEmployedData TechnologyEmployed { get; set; }
        public string ReasonForExport { get; set; }

        public RecoveryOperationViewModel()
        {
        }

        public RecoveryOperationViewModel(RecoveryOperationInfo recoveryOperationInfo)
        {
            NotificationId = recoveryOperationInfo.NotificationId;
            NotificationType = recoveryOperationInfo.NotificationType;
            Progress = recoveryOperationInfo.Progress;
            PreconstedAnswer = recoveryOperationInfo.PreconstedAnswer;
            OperationCodes = recoveryOperationInfo.OperationCodes.OrderBy(c => c.Value).Select(c => c.Code).ToList();
            TechnologyEmployed = recoveryOperationInfo.TechnologyEmployed;
            ReasonForExport = recoveryOperationInfo.ReasonForExport;
        }
    }
}