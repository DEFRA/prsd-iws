namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.TechnologyEmployed;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class RecoveryOperationInfoMap : IMap<NotificationApplication, RecoveryOperationInfo>
    {
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap;
        private readonly IMap<NotificationApplication, string> preconsentedAnswerMap;
        private readonly IMap<NotificationApplication, TechnologyEmployedData> technologyEmployedMap;

        public RecoveryOperationInfoMap(
            IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap,
            IMap<NotificationApplication, string> preconsentedAnswerMap,
            IMap<NotificationApplication, TechnologyEmployedData> technologyEmployedMap)
        {
            this.completionProgressMap = completionProgressMap;
            this.preconsentedAnswerMap = preconsentedAnswerMap;
            this.technologyEmployedMap = technologyEmployedMap;
        }

        public RecoveryOperationInfo Map(NotificationApplication notification)
        {
            return new RecoveryOperationInfo
            {
                NotificationId = notification.Id,
                NotificationType = notification.NotificationType == NotificationType.Disposal
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
                Progress = completionProgressMap.Map(notification),
                PreconstedAnswer = preconsentedAnswerMap.Map(notification),
                TechnologyEmployed = technologyEmployedMap.Map(notification),
                ReasonForExport = notification.ReasonForExport ?? string.Empty,
                OperationCodes = GetOperationCodes(notification)
            };
        }

        private static List<OperationCodeData> GetOperationCodes(NotificationApplication notification)
        {
            var operationCodes = new List<OperationCodeData>();
            foreach (var operationInfo in notification.OperationInfos)
            {
                var ocd = new OperationCodeData
                {
                    Code = operationInfo.OperationCode.DisplayName,
                    Value = operationInfo.OperationCode.Value
                };
                operationCodes.Add(ocd);
            }
            return operationCodes;
        }
    }
}
