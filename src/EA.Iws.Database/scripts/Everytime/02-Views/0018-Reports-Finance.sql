IF OBJECT_ID('[Reports].[Finance]') IS NULL
    EXEC('CREATE VIEW [Reports].[Finance] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Finance]
AS
    SELECT 
        NO.NotificationNumber,
        CASE WHEN IU.Id IS NULL THEN 'External' ELSE 'Internal' END AS CreatedBy,
        NO.Exporter AS [Notifier],
        NO.ExporterAddress AS [NotifierAddress],
        NO.ExporterPostalCode AS [NotifierPostalCode],
        NO.Importer AS [Consignee],
        NO.ImporterAddress AS [ConsigneeAddress],
        NO.ImporterPostalCode AS [ConsigneePostalCode],
        NO.Facility,
        NO.FacilityAddress,
        NO.FacilityPostalCode,
        NA.PaymentReceivedDate,
        N.Charge AS [TotalBillable],
        P.TotalPaid,
        P.LatestPaymentDate,
        CASE WHEN P.TotalPaid IS NULL THEN NULL ELSE (SELECT PotentialRefund FROM [Reports].[PricingInfo](N.Id)) END AS AmountToRefund,
        P.TotalRefunded,
        P.LatestRefundDate,
        N.NumberOfShipments AS [IntendedNumberOfShipments],
        N.IntendedQuantity,
        N.Units,
        (SELECT COUNT(MovementId) FROM [Reports].[Movements] WHERE NotificationId = NO.Id) AS [TotalShipmentsMade],
        N.ImportOrExport,
        N.Type AS NotificationType,
        CAST(N.Preconsented AS BIT) AS [Preconsented],
        CAST(NO.HasMultipleFacilities AS BIT) AS [HasMultipleFacilities],
        NA.ConsentFrom,
        NA.ConsentTo,
        NA.Status,
        N.CompetentAuthorityId,
        NA.ReceivedDate,
		CASE WHEN FC.IsInterim IS NULL THEN InS.IsInterim ELSE FC.IsInterim END AS IsInterim
    FROM [Reports].[NotificationOrganisations] NO
    INNER JOIN [Reports].[NotificationAssessment] NA ON NO.Id = NA.NotificationId
    INNER JOIN [Reports].[Notification] N ON NO.Id = N.Id
    LEFT JOIN [Reports].[Payments] P ON NO.Id = P.NotificationId
    LEFT JOIN [Person].[InternalUser] IU ON N.[UserId] = IU.[UserId]
	LEFT JOIN [Notification].[FacilityCollection] FC ON NO.Id = FC.NotificationId
	LEFT JOIN [ImportNotification].[InterimStatus] InS ON NO.Id = InS.ImportNotificationId
    WHERE 
        (NA.[ExportStatusId] IS NULL OR NA.[ExportStatusId] <> 1 OR (NA.[ExportStatusId] = 1 AND IU.[UserId] IS NOT NULL))
        AND (NA.[ImportStatusId] IS NULL OR NA.[ImportStatusId] > 2)
GO