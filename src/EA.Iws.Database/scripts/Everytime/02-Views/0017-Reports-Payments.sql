﻿IF OBJECT_ID('[Reports].[Payments]') IS NULL
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
        (SELECT TOP 1 [Date] FROM [Notification].[Transaction] WHERE NotificationId = N.Id AND Debit IS NOT NULL ORDER BY [Date] DESC) AS [LatestRefundDate]
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
        (SELECT TOP 1 [Date] FROM [ImportNotification].[Transaction] WHERE NotificationId = N.Id AND Debit IS NOT NULL ORDER BY [Date] DESC) AS [LatestRefundDate]
    FROM [ImportNotification].[Transaction] T
    INNER JOIN [ImportNotification].[Notification] N ON N.Id = T.NotificationId
    GROUP BY N.Id, N.NotificationNumber

GO