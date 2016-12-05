namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Reports;

    public interface IMissingShipmentsRepository
    {
        Task<IEnumerable<MissingShipment>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority, MissingShipmentsReportDates dateType);
    }
}
