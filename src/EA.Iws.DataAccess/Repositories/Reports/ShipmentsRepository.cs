﻿namespace EA.Iws.DataAccess.Repositories.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Reports;
    using Domain.Reports;

    internal class ShipmentsRepository : IShipmentsRepository
    {
        private readonly IwsContext context;

        public ShipmentsRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Shipment>> Get(DateTime from, DateTime to, UKCompetentAuthority competentAuthority, 
            ShipmentsReportDates dateType, ShipmentReportTextFields? textFieldType,
            TextFieldOperator? textFieldOperatorType, string textSearch)
        {
            var textFilter = TextFilterHelper.GetTextFilter(textFieldType, textFieldOperatorType, textSearch);
            textFilter = !string.IsNullOrEmpty(textFilter) ? string.Format("AND {0}", textFilter) : string.Empty;

            var query = @"SELECT DISTINCT 
                    [NotificationNumber],
                    [ImportOrExport],
                    [Exporter],
                    [Importer],
                    [Facility],
                    [BaselOecdCode],
                    [ShipmentNumber],
                    [ActualDateOfShipment],
                    [ConsentFrom],
                    [ConsentTo],
                    [PrenotificationDate] AS [PaperworkUploaded],
                    [ReceivedDate],
                    [CompletedDate],
                    [QuantityReceived],
                    [QuantityReceivedUnitId] AS [Units],
                    [ChemicalCompositionTypeId],
                    [ChemicalComposition],
                    [LocalArea],
                    [TotalQuantity],
                    [TotalQuantityUnitsId],
                    [EntryPort],
                    [DestinationCountry],
                    [ExitPort],
                    [OriginatingCountry],
                    [Status],
                    [EwcCodes],
                    [OperationCodes],
                    CASE WHEN YCode IS NULL THEN 'NA' ELSE YCode END AS [YCode],
                    CASE WHEN HCode IS NULL THEN 'NA' ELSE HCode END AS [HCode],
                    CASE WHEN UNClass IS NULL THEN 'NA' ELSE UNClass END AS [UNClass]
                FROM [Reports].[ShipmentsCache]
                WHERE [CompetentAuthorityId] = @ca
                AND (@dateType = 'NotificationReceivedDate' and  [NotificationReceivedDate] BETWEEN @from AND @to
                     OR @dateType = 'ConsentFrom' and  [ConsentFrom] BETWEEN @from AND @to
                     OR @dateType = 'ConsentTo' and  [ConsentTo] BETWEEN @from AND @to
                     OR @dateType = 'ReceivedDate' and  [ReceivedDate] BETWEEN @from AND @to
                     OR @dateType = 'CompletedDate' and  [CompletedDate] BETWEEN @from AND @to
                     OR @dateType = 'ActualDateOfShipment' and  [ActualDateOfShipment] BETWEEN @from AND @to
                     OR @dateType = 'RejectedShipmentDate' and  [RejectedShipmentDate] BETWEEN @from AND @to)
                {0}
                ORDER BY
                    [NotificationNumber],
                    [ShipmentNumber]";

            return await context.Database.SqlQuery<Shipment>(string.Format(query, textFilter),
                new SqlParameter("@from", from),
                new SqlParameter("@to", to),
                new SqlParameter("@ca", (int)competentAuthority),
                new SqlParameter("@dateType", dateType.ToString())).ToArrayAsync();
        }
    }
}
