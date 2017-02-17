IF OBJECT_ID('[Reports].[Movements]') IS NULL
    EXEC('CREATE VIEW [Reports].[Movements] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Movements]
AS
    SELECT
        N.Id AS [NotificationId],
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        M.Id AS [MovementId],
        M.Number AS [ShipmentNumber],
        CAST(M.Date AS DATE) AS [ActualDateOfShipment],
        CAST(M.PrenotificationDate AS DATE) AS [PrenotificationDate],
        M.Status,
        MS.Status AS [StatusName],
        MD.Quantity AS [ActualQuantity],
        MD_U.Description AS [ActualQuantityUnit],
        MR.Quantity AS [QuantityReceived],
        MR_U.Description AS [QuantityReceivedUnit],
        MR_U.Id AS [QuantityReceivedUnitId],
        CAST(MR.Date AS DATE) AS [ReceivedDate],
        CAST(MOR.Date AS DATE) AS [CompletedDate],
        'Export' AS [ImportOrExport]

    FROM		[Notification].[Movement] AS M

    INNER JOIN	[Lookup].[MovementStatus] AS MS 
    ON			[M].[Status] = [MS].[Id]

    INNER JOIN	[Notification].[Notification] AS N 
    ON			[M].[NotificationId] = [N].[Id]

    LEFT JOIN	[Notification].[MovementDetails] AS MD
    ON			[M].[Id] = [MD].[MovementId]
     
    LEFT JOIN 	[Lookup].[ShipmentQuantityUnit] AS MD_U 
    ON			[MD].[Unit] = [MD_U].[Id]
    
    LEFT JOIN	[Notification].[MovementReceipt] AS MR
    ON			[M].[Id] = [MR].[MovementId]

    LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS MR_U 
    ON			[MR].[Unit] = [MR_U].[Id]

    LEFT JOIN	[Notification].[MovementOperationReceipt] AS MOR
    ON			[M].[Id] = [MOR].[MovementId]

    UNION ALL

    SELECT
        N.Id AS [NotificationId],
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        M.Id AS [MovementId],
        M.Number AS [ShipmentNumber],
        CAST(M.ActualShipmentDate AS DATE) AS [ActualDateOfShipment],
        CAST(M.PrenotificationDate AS DATE) AS [PrenotificationDate],
        MS.Id AS [Status],
        MS.Status AS [StatusName],
        NULL AS [ActualQuantity],
        NULL AS [ActualQuantityUnit],
        MR.Quantity AS [QuantityReceived],
        MR_U.Description AS [QuantityReceivedUnit],
        MR_U.Id AS [QuantityReceivedUnitId],
        CAST(MR.Date AS DATE) AS [ReceivedDate],
        CAST(MOR.Date AS DATE) AS [CompletedDate],
        'Import' AS [ImportOrExport]

    FROM		[ImportNotification].[Movement] AS M
    
    INNER JOIN	[ImportNotification].[Notification] AS N 
    ON			[M].[NotificationId] = [N].[Id]
    
    LEFT JOIN	[ImportNotification].[MovementReceipt] AS MR
    ON			[M].[Id] = [MR].[MovementId]

    LEFT JOIN	[Lookup].[ShipmentQuantityUnit] AS MR_U 
    ON			[MR].[Unit] = [MR_U].[Id]

    LEFT JOIN	[ImportNotification].[MovementOperationReceipt] AS MOR
    ON			[M].[Id] = [MOR].[MovementId]

    INNER JOIN  [Lookup].[MovementStatus] MS 
    ON			[MS].[Id] = CASE 
                    WHEN M.IsCancelled = 1 THEN 6
                    WHEN MOR.Date IS NOT NULL THEN 4
                    WHEN MR.Date IS NOT NULL THEN 3
                    ELSE 7
                END

GO