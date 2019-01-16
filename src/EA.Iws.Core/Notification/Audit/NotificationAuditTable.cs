namespace EA.Iws.Core.Notification.Audit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NotificationAuditTable
    {
        public List<NotificationAuditForDisplay> TableData { get; set; }

        //COULLM: rename this property
        public int NumberOfShipments { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
