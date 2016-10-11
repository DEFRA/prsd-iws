PRINT 'Creating Customs Office tables'

CREATE TABLE [Notification].[EntryCustomsOffice]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_EntryCustomsOffice PRIMARY KEY,
	[Name] NVARCHAR(1024) NOT NULL,
	[Address] NVARCHAR(4000) NOT NULL,
	[CountryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_EntryCustomsOffice_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[NotificationId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_EntryCustomsOffice_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id]),
	[RowVersion] ROWVERSION NOT NULL
);
GO

CREATE TABLE [Notification].[ExitCustomsOffice]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_ExitCustomsOffice PRIMARY KEY,
	[Name] NVARCHAR(1024) NOT NULL,
	[Address] NVARCHAR(4000) NOT NULL,
	[CountryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_ExitCustomsOffice_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[NotificationId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_ExitCustomsOffice_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id]),
	[RowVersion] ROWVERSION NOT NULL
);
GO

PRINT 'Adding indexes to created tables.'
CREATE NONCLUSTERED INDEX IX_EntryCustomsOffice_NotificationId
ON [Notification].[EntryCustomsOffice] ([NotificationId])
GO

CREATE NONCLUSTERED INDEX IX_ExitCustomsOffice_NotificationId
ON [Notification].[ExitCustomsOffice] ([NotificationId])
GO