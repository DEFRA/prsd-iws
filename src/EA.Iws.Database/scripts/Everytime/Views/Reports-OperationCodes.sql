IF OBJECT_ID('[Reports].[OperationCodes]') IS NULL
    EXEC('CREATE VIEW [Reports].[OperationCodes] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[OperationCodes]
AS
	SELECT
		N.Id AS NotificationId,
		N.NotificationNumber,
		O.Id as OperationCodeId,
		O.Name
	FROM [Notification].[OperationCodes] OC
	INNER JOIN [Notification].[Notification] N ON OC.NotificationId = N.Id
	INNER JOIN [Lookup].[OperationCode] O ON OC.OperationCode = O.Id
GO