CREATE SCHEMA [ImportNotification] AUTHORIZATION [dbo];

GO

ALTER SCHEMA [ImportNotification] 
    TRANSFER [Notification].[ImportMovement];

GO

ALTER SCHEMA [ImportNotification] 
    TRANSFER [Notification].[ImportNotification];

GO

EXEC sp_rename 'ImportNotification.ImportMovement', 'Movement';
GO

EXEC sp_rename 'ImportNotification.ImportNotification', 'Notification';
GO