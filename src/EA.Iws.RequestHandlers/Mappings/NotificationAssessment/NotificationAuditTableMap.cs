namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class NotificationAuditTableMap : IMap<IEnumerable<Audit>, NotificationAuditTable>
    {
        private readonly IMapper mapper;

        public NotificationAuditTableMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public NotificationAuditTable Map(IEnumerable<Audit> notificationAudits)
        {
            return new NotificationAuditTable
            {
                TableData = notificationAudits
                    .Select(updateHistoryItem => mapper.Map<NotificationAuditForDisplay>(updateHistoryItem))
                    .OrderByDescending(x => x.DateAdded)
                    .ToList()
            };
        }
    }
}
