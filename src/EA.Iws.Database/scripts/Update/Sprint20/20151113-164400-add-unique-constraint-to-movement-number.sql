DECLARE @DeletedMovements TABLE ([Id] UNIQUEIDENTIFIER);
DECLARE @DeletedMovementDetails TABLE ([Id] UNIQUEIDENTIFIER);

INSERT INTO @DeletedMovements
SELECT [Id]
FROM [Notification].[Movement]
WHERE [NotificationId] IN (
	SELECT [NotificationId]
	FROM [Notification].[Movement]
	GROUP BY [NotificationId], [Number]
	HAVING COUNT(*) > 1);

INSERT INTO @DeletedMovementDetails
SELECT [Id]
FROM [Notification].[MovementDetails]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[MovementCarrier]
WHERE [MovementDetailsId] IN (SELECT [Id] FROM @DeletedMovementDetails);

DELETE FROM [Notification].[MovementPackagingInfo]
WHERE [MovementDetailsId] IN (SELECT [Id] FROM @DeletedMovementDetails);

DELETE FROM [Notification].[MovementDetails]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[MovementReceipt]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[MovementOperationReceipt]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[MovementStatusChange]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[Movement]
WHERE [Id] IN (SELECT [Id] FROM @DeletedMovements);
GO

ALTER TABLE [Notification].[Movement]
ADD CONSTRAINT UQ_Movement_NotificationId_Number UNIQUE ([NotificationId], [Number]);
GO