IF OBJECT_ID('[Reports].[Payments]') IS NULL
    EXEC('CREATE VIEW [Reports].[Payments] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[Payments]
AS
    SELECT 
        N.Id AS [NotificationId],
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        SUM(T.Credit) AS TotalPaid,
        SUM(T.Debit) AS TotalRefunded,
        (SELECT TOP 1 [Date] FROM [Notification].[Transaction] WHERE NotificationId = N.Id AND Credit IS NOT NULL ORDER BY [Date] DESC) AS [LatestPaymentDate],
        (SELECT TOP 1 [Date] FROM [Notification].[Transaction] WHERE NotificationId = N.Id AND Debit IS NOT NULL ORDER BY [Date] DESC) AS [LatestRefundDate],
		(SELECT CASE WHEN RIGHT(LTRIM(RTRIM(T2.Comments)),1) = '.' THEN LTRIM(RTRIM(T2.Comments)) + ' ' ELSE LTRIM(RTRIM(T2.Comments)) + '. ' END AS Comments
          FROM [Notification].[Transaction] T2
          WHERE T2.NotificationId = N.Id
		  ORDER BY [Date] ASC
          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') AS Comments
    FROM [Notification].[Transaction] T
    INNER JOIN [Notification].[Notification] N ON N.Id = T.NotificationId
    GROUP BY N.Id, N.NotificationNumber

    UNION ALL

    SELECT 
        N.Id AS [NotificationId],
        REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
        SUM(T.Credit) AS TotalPaid,
        SUM(T.Debit) AS TotalRefunded,
        (SELECT TOP 1 [Date] FROM [ImportNotification].[Transaction] WHERE NotificationId = N.Id AND Credit IS NOT NULL ORDER BY [Date] DESC) AS [LatestPaymentDate],
        (SELECT TOP 1 [Date] FROM [ImportNotification].[Transaction] WHERE NotificationId = N.Id AND Debit IS NOT NULL ORDER BY [Date] DESC) AS [LatestRefundDate],
		(SELECT CASE WHEN RIGHT(LTRIM(RTRIM(T2.Comments)),1) = '.' THEN LTRIM(RTRIM(T2.Comments)) + ' ' ELSE LTRIM(RTRIM(T2.Comments)) + '. ' END AS Comments
          FROM [ImportNotification].[Transaction] T2
          WHERE T2.NotificationId = N.Id
		  ORDER BY [Date] ASC
          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') AS Comments
    FROM [ImportNotification].[Transaction] T
    INNER JOIN [ImportNotification].[Notification] N ON N.Id = T.NotificationId
    GROUP BY N.Id, N.NotificationNumber
GO