namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.Shared;
    using Core.TechnologyEmployed;

    public class RecoveryOperationInfo
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationApplicationCompletionProgress Progress { get; set; }
        public string PreconstedAnswer { get; set; }
        public List<OperationCodeData> OperationCodes { get; set; }
        public TechnologyEmployedData TechnologyEmployed { get; set; }
        public string ReasonForExport { get; set; }
    }
}
