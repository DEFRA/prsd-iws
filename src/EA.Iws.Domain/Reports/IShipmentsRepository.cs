namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Reports;

    public interface IShipmentsRepository
    {
        Task<IEnumerable<MissingShipment>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority, ShipmentsReportDates dateType);
    }
}
