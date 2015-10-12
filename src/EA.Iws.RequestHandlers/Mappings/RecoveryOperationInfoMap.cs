namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using Core.Notification.Overview;
    using Core.OperationCodes;
    using Core.TechnologyEmployed;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class RecoveryOperationInfoMap : IMap<NotificationApplication, RecoveryOperation>
    {
        private readonly IMap<NotificationApplication, string> preconsentedAnswerMap;
        private readonly IMap<NotificationApplication, TechnologyEmployedData> technologyEmployedMap;

        public RecoveryOperationInfoMap(
            IMap<NotificationApplication, string> preconsentedAnswerMap,
            IMap<NotificationApplication, TechnologyEmployedData> technologyEmployedMap)
        {
            this.preconsentedAnswerMap = preconsentedAnswerMap;
            this.technologyEmployedMap = technologyEmployedMap;
        }

        public RecoveryOperation Map(NotificationApplication notification)
        {
            return new RecoveryOperation
            {
                NotificationId = notification.Id,
                NotificationType = notification.NotificationType == NotificationType.Disposal
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
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
