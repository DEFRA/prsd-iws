namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification.Overview;
    using Core.OperationCodes;
    using Core.Shared;
    using Core.TechnologyEmployed;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class RecoveryOperationInfoMap : IMap<NotificationApplication, RecoveryOperation>
    {
        private readonly IMap<NotificationApplication, string> preconsentedAnswerMap;
        private readonly IMap<TechnologyEmployed, TechnologyEmployedData> technologyEmployedMap;
        private readonly ITechnologyEmployedRepository technologyEmployedRepository;

        public RecoveryOperationInfoMap(
            IMap<NotificationApplication, string> preconsentedAnswerMap,
            IMap<TechnologyEmployed, TechnologyEmployedData> technologyEmployedMap,
            ITechnologyEmployedRepository technologyEmployedRepository)
        {
            this.preconsentedAnswerMap = preconsentedAnswerMap;
            this.technologyEmployedMap = technologyEmployedMap;
            this.technologyEmployedRepository = technologyEmployedRepository;
        }

        public RecoveryOperation Map(NotificationApplication notification)
        {
            var technologyEmployed =
                Task.Run(() => technologyEmployedRepository.GetByNotificaitonId(notification.Id)).Result;

            return new RecoveryOperation
            {
                NotificationId = notification.Id,
                NotificationType = notification.NotificationType == NotificationType.Disposal
                        ? NotificationType.Disposal
                        : NotificationType.Recovery,
                PreconstedAnswer = preconsentedAnswerMap.Map(notification),
                TechnologyEmployed = technologyEmployedMap.Map(technologyEmployed),
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
