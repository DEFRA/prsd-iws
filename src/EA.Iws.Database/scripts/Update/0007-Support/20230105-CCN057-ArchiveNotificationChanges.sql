ALTER TABLE [ImportNotification].[Notification] ADD IsArchived BIT NOT NULL DEFAULT 0;
ALTER TABLE [ImportNotification].[Notification] ADD ArchivedDate date NULL;
ALTER TABLE [ImportNotification].[Notification] ADD ArchivedByUserId nvarchar(128) NULL;

ALTER TABLE [ImportNotification].[Notification] WITH CHECK ADD CONSTRAINT [FK_ImportNotification_User] FOREIGN KEY([ArchivedByUserId])
REFERENCES  [Identity].[AspNetUsers] ([Id])
GO

ALTER TABLE [Notification].[Notification] ADD IsArchived BIT NOT NULL DEFAULT 0;
ALTER TABLE [Notification].[Notification] ADD ArchivedDate date NULL;
ALTER TABLE [Notification].[Notification] ADD ArchivedByUserId  nvarchar(128) NULL;

ALTER TABLE [Notification].[Notification] WITH CHECK ADD CONSTRAINT [FK_Notification_User] FOREIGN KEY([ArchivedByUserId])
REFERENCES  [Identity].[AspNetUsers] ([Id])
GO
