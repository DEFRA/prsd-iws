CREATE TABLE [ImportNotification].[Exporter](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT PK_ImportNotificationExporter PRIMARY KEY,
	[ImportNotificationId] [uniqueidentifier] NOT NULL 
		CONSTRAINT FK_ImportNotificationExporter_ImportNotification FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
	[Name] [nvarchar](3000) NOT NULL,
	[Address1] [nvarchar](1024) NOT NULL,
	[Address2] [nvarchar](1024) NULL,
	[TownOrCity] [nvarchar](1024) NOT NULL,
	[PostalCode] [nvarchar](64) NULL,
	[CountryId] [uniqueidentifier] NOT NULL
		CONSTRAINT FK_ImportNotificationExporter_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[ContactName] [nvarchar](1024) NOT NULL,
	[Telephone] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[RowVersion] [timestamp] NULL
)