namespace EA.Iws.Domain.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Reports;
    using Core.WasteType;

    public interface IShipmentsRepository
    {
        Task<IEnumerable<Shipment>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority,
            ShipmentsReportDates dateType, ShipmentReportTextFields? textFieldType,
            TextFieldOperator? textFieldOperatorType, string textSearch);
    }
}
