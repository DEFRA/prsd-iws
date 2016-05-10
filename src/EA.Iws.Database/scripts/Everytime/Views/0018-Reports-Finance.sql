IF OBJECT_ID('[Reports].[Finance]') IS NULL
    EXEC('CREATE VIEW [Reports].[Finance] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Finance]
AS
    SELECT 
        NO.NotificationNumber,
        NO.Exporter AS [Notifier],
        NO.ExporterAddress AS [NotifierAddress],
        NO.Importer AS [Consignee],
        NO.ImporterAddress AS [ConsigneeAddress],
        NO.Facility,
        NO.FacilityAddress,
        NA.PaymentReceivedDate,
        N.Charge AS [TotalBillable],
        P.TotalPaid,
        P.LatestPaymentDate,
        CASE WHEN P.TotalPaid IS NULL THEN NULL ELSE [Reports].[PotentialRefund](N.Id) END AS AmountToRefund,
        P.TotalRefunded,
        P.LatestRefundDate,
        N.NumberOfShipments AS [IntendedNumberOfShipments],
        (SELECT COUNT(MovementId) FROM [Reports].[Movements] WHERE NotificationId = NO.Id) AS [TotalShipmentsMade],
        N.ImportOrExport,
        N.Type AS NotificationType,
        CAST(N.Preconsented AS BIT) AS [Preconsented],
        CAST(NO.HasMultipleFacilities AS BIT) AS [HasMultipleFacilities],
        NA.ConsentFrom,
        NA.ConsentTo,
        NA.Status,
        N.CompetentAuthorityId
    FROM [Reports].[NotificationOrganisations] NO
    INNER JOIN [Reports].[NotificationAssessment] NA ON NO.Id = NA.NotificationId
    INNER JOIN [Reports].[Notification] N ON NO.Id = N.Id
    LEFT JOIN [Reports].[Payments] P ON NO.Id = P.NotificationId
    WHERE 
        (NA.[ExportStatusId] IS NULL OR NA.[ExportStatusId] <> 1)
        AND (NA.[ImportStatusId] IS NULL OR NA.[ImportStatusId] > 2)
GO