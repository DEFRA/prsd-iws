IF OBJECT_ID('[Reports].[NotificationShipmentDataMissingShipments]') IS NULL
    EXEC('CREATE VIEW [Reports].[NotificationShipmentDataMissingShipments] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[NotificationShipmentDataMissingShipments]
AS
	
	SELECT	
		N.Id AS [NotificationId],
		N.NotificationNumber,
		N.CompetentAuthorityId,
		O.Exporter,
		O.Importer,
		O.Facility,
		M.ShipmentNumber,
		M.ActualDateOfShipment,
		N.ConsentFrom,
		N.ConsentTo,
		M.PrenotificationDate,
		M.ReceivedDate,
		M.CompletedDate,
		M.QuantityReceived,
		M.QuantityReceivedUnit,
		M.QuantityReceivedUnitId,
		W.ChemicalCompositionType + ' - ' + W.ChemicalCompositionDescription AS [ChemicalComposition],
		N.LocalArea

	FROM		[Reports].[Notification] AS N

	INNER JOIN	[Reports].[Movements] AS M
	ON			[M].[NotificationId] = [N].[Id]

	INNER JOIN	[Reports].[NotificationOrganisations] AS O
	ON			[O].[Id] = [N].[Id]

	LEFT JOIN	[Reports].[WasteType] AS W
	ON			[N].[Id] = [W].[NotificationId]

GO