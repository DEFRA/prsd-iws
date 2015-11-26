IF OBJECT_ID('[Reports].[Movements]') IS NULL
    EXEC('CREATE VIEW [Reports].[Movements] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Movements]
AS
	SELECT
		N.Id AS NotificationId,
		N.NotificationNumber,
		M.Id AS MovementId,
		M.Number AS ShipmentNumber,
		M.Date AS ActualDateOfShipment,
		M.Status,
		MS.Status AS StatusName,
		MD.Quantity AS ActualQuantity,
		MD_U.Description AS ActualQuantityUnit,
		MR.Quantity AS QuantityReceived,
		MR_U.Description AS QuantityReceivedUnit,
		MR.Date AS ReceivedDate
	FROM [Notification].[Movement] M
	INNER JOIN [Lookup].[MovementStatus] MS ON M.Status = MS.Id
	INNER JOIN [Notification].[Notification] N ON M.NotificationId = N.Id
	LEFT JOIN [Notification].[MovementDetails] MD 
		INNER JOIN [Lookup].[ShipmentQuantityUnit] MD_U ON MD.Unit = MD_U.Id 
	ON MD.MovementId = M.Id
	LEFT JOIN [Notification].[MovementReceipt] MR 
		INNER JOIN [Lookup].[ShipmentQuantityUnit] MR_U ON MR.Unit = MR_U.Id
	ON MR.MovementId = M.Id
GO