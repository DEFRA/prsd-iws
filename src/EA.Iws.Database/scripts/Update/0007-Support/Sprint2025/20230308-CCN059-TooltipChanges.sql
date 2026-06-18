ALTER TABLE [Notification].[Exporter] ADD IsUkBased BIT NOT NULL DEFAULT 1;
ALTER TABLE [ImportNotification].[Facility] ADD AdditionalRegistrationNumber NVARCHAR(100) NULL;