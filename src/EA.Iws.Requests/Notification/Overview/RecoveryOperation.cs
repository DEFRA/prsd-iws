namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using System.Collections.Generic;
    using Core.OperationCodes;
    using Core.Shared;
    using Core.TechnologyEmployed;

    public class RecoveryOperation
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsPreconsentStatusChosen { get; set; }
        public bool AreOperationCodesChosen { get; set; }
        public bool IsTechnologyEmployedCompleted { get; set; }
        public bool IsReasonForExportCompleted { get; set; }
        public string PreconstedAnswer { get; set; }
        public List<OperationCodeData> OperationCodes { get; set; }
        public TechnologyEmployedData TechnologyEmployed { get; set; }
        public string ReasonForExport { get; set; }
    }
}
