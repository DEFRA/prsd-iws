CREATE TABLE [ImportNotification].[FacilityCollection](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT PK_ImportNotificationFacilityCollection PRIMARY KEY,
	[ImportNotificationId] [uniqueidentifier] NOT NULL 
		CONSTRAINT FK_ImportNotificationFacilityCollection_ImportNotification FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
	[AllFacilitiesPreconsented] BIT NOT NULL,
	[RowVersion] [timestamp] NOT NULL
);

GO

CREATE TABLE [ImportNotification].[Facility](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT PK_ImportNotificationFacility PRIMARY KEY,
	[FacilityCollectionId] [uniqueidentifier] NOT NULL 
		CONSTRAINT FK_ImportNotificationFacility_FacilityCollection FOREIGN KEY REFERENCES [ImportNotification].[FacilityCollection]([Id]),
	[Name] [nvarchar](3000) NOT NULL,
	[Type] [int] NOT NULL
		CONSTRAINT FK_ImportNotificationFacility_BusinessType FOREIGN KEY REFERENCES [Lookup].[BusinessType]([Id]),
	[RegistrationNumber] [nvarchar](100) NOT NULL,
	[Address1] [nvarchar](1024) NOT NULL,
	[Address2] [nvarchar](1024) NULL,
	[TownOrCity] [nvarchar](1024) NOT NULL,
	[PostalCode] [nvarchar](64) NULL,
	[CountryId] [uniqueidentifier] NOT NULL
		CONSTRAINT FK_ImportNotificationFacility_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[ContactName] [nvarchar](1024) NOT NULL,
	[Telephone] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[RowVersion] [timestamp] NOT NULL
);

GO