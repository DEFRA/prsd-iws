namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Notification.Overview;
    using Core.OperationCodes;
    using Core.Shared;
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
                        ? NotificationType.Disposal
                        : NotificationType.Recovery,
                PreconstedAnswer = preconsentedAnswerMap.Map(notification),
                TechnologyEmployed = technologyEmployedMap.Map(notification),
                ReasonForExport = notification.ReasonForExport ?? string.Empty,
                OperationCodes = GetOperationCodes(notification)
            };
        }

        private static List<OperationCode> GetOperationCodes(NotificationApplication notification)
        {
            return notification.OperationInfos.Select(o => o.OperationCode).ToList();
        }
    }
}
