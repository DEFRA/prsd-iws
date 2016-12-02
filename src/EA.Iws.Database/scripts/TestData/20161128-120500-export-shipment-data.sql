DECLARE @notificationId uniqueidentifier;

select @notificationId = [Id] FROM [Notification].[Notification] WHERE NotificationNumber = 'GB 0001 002504' 

DECLARE @shipmentNumber INT;
SET @shipmentNumber = 1;

WHILE @shipmentNumber < 5
BEGIN

	INSERT INTO [Notification].[Movement]
			([Id],[Number],[NotificationId],[Date],[Status],[PrenotificationDate])
		   VALUES
			(NEWID(),@shipmentNumber,@notificationId,Cast(N'2016-11-01' AS DATE),2,Cast(N'2016-10-26' AS DATE));

	SET @shipmentNumber = @shipmentNumber + 1;

END;

DECLARE @movementId uniqueidentifier

SELECT @movementId = [Id] FROM [Notification].[Movement]
WHERE NotificationId = @notificationId and Number = 1

INSERT INTO [Notification].[MovementReceipt]([Id], [MovementId], [Date], [Quantity], [Unit])
VALUES (NEWID(), @movementId, Cast(N'2016-11-10' AS DATE), 1, 3)

UPDATE [Notification].[Movement]
SET [Status] = 3
WHERE [Id] = @movementId


SELECT @movementId = [Id] FROM [Notification].[Movement]
WHERE NotificationId = @notificationId and Number = 2

INSERT INTO [Notification].[MovementReceipt]([Id], [MovementId], [Date], [Quantity], [Unit])
VALUES (NEWID(), @movementId, Cast(N'2016-11-10' AS DATE), 1, 3)

INSERT INTO [Notification].[MovementOperationReceipt]([Id], [MovementId], [Date])
VALUES (NEWID(), @movementId, Cast(N'2016-11-15' AS DATE))

UPDATE [Notification].[Movement]
SET [Status] = 4
WHERE [Id] = @movementId