INSERT INTO [Lookup].[MovementStatus] ([Id], [Status])
VALUES (7, 'Captured');
GO

ALTER TABLE [Notification].[Movement]
ADD CONSTRAINT UX_Movement_Number UNIQUE (NotificationId, Number)
GO