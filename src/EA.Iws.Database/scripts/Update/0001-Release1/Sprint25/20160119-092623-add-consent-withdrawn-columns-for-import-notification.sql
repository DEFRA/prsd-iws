ALTER TABLE [ImportNotification].[NotificationDates]
ADD ConsentWithdrawnDate DATETIMEOFFSET NULL;
GO

ALTER TABLE [ImportNotification].[NotificationDates]
ADD ConsentWithdrawnReasons NVARCHAR(4000) NULL;
GO

INSERT INTO [Lookup].[ImportNotificationStatus]
(
	[Id], 
	[Description]
)
VALUES (10, 'Consent withdrawn');

GO

ALTER TABLE [ImportNotification].[NotificationDates]
ADD ObjectedDate DATETIMEOFFSET NULL;
GO

ALTER TABLE [ImportNotification].[NotificationDates]
ADD ObjectedReason NVARCHAR(4000) NULL;
GO

INSERT INTO [Lookup].[ImportNotificationStatus]
(
	[Id], 
	[Description]
)
VALUES (11, 'Objected');

GO