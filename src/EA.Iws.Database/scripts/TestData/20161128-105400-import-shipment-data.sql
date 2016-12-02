DECLARE @notificationId uniqueidentifier;

select @notificationId = [Id] FROM [ImportNotification].[Notification] WHERE NotificationNumber = 'GB 0001 007701' 

DECLARE @shipmentNumber INT;
SET @shipmentNumber = 1;

WHILE @shipmentNumber < 5
BEGIN

	INSERT INTO [ImportNotification].[Movement]
			([Id],[Number],[NotificationId],[ActualShipmentDate],[IsCancelled])
		   VALUES
			(NEWID(),@shipmentNumber,@notificationId,Cast(N'2016-11-01' AS DATE),'false');

	SET @shipmentNumber = @shipmentNumber + 1;

END;

DECLARE @movementId uniqueidentifier

SELECT @movementId = [Id] FROM [ImportNotification].[Movement]
WHERE NotificationId = @notificationId and Number = 1

INSERT INTO [ImportNotification].[MovementReceipt]([Id], [MovementId], [Date], [Quantity], [Unit])
VALUES (NEWID(), @movementId, Cast(N'2016-11-10' AS DATE), 0.01, 1)



SELECT @movementId = [Id] FROM [ImportNotification].[Movement]
WHERE NotificationId = @notificationId and Number = 2

INSERT INTO [ImportNotification].[MovementReceipt]([Id], [MovementId], [Date], [Quantity], [Unit])
VALUES (NEWID(), @movementId, Cast(N'2016-11-10' AS DATE), 0.01, 1)

INSERT INTO [ImportNotification].[MovementOperationReceipt]([Id], [MovementId], [Date])
VALUES (NEWID(), @movementId, Cast(N'2016-11-15' AS DATE))