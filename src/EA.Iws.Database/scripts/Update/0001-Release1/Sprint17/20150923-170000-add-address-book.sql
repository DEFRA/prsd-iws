CREATE TABLE [Lookup].[AddressBookEntityType]
(
	Id INT CONSTRAINT PK_AddressBookEntityType PRIMARY KEY NOT NULL,
	Description NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[AddressBookEntityType] (Id, Description)
VALUES 
(1, 'Producer'), 
(2, 'Carrier');
GO

CREATE TABLE [Lookup].[BusinessType]
(
	Id INT CONSTRAINT PK_BusinessType PRIMARY KEY NOT NULL,
	Description NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[BusinessType] (Id, Description)
VALUES 
(1, 'Limited Company'),
(2, 'Sole Trader'),
(3, 'Partnership'),
(4, 'Other');
GO

CREATE TABLE [Person].[AddressBook]
(
	Id UNIQUEIDENTIFIER CONSTRAINT PK_AddressBook PRIMARY KEY NOT NULL,
	Type INT NOT NULL CONSTRAINT FK_AddressBook_AddressBookEntityType FOREIGN KEY REFERENCES [Lookup].[AddressBookEntityType]([Id]),
	UserId UNIQUEIDENTIFIER NOT NULL,
	RowVersion ROWVERSION NOT NULL
);
GO

CREATE TABLE [Person].[AddressBookRecord]
(
	Id UNIQUEIDENTIFIER CONSTRAINT PK_AddressBookRecord PRIMARY KEY NOT NULL,
	AddressBookId UNIQUEIDENTIFIER CONSTRAINT FK_AddressBookRecord_AddressBook FOREIGN KEY REFERENCES [Person].[AddressBook]([Id]),
	[Name]	NVARCHAR(3000) NOT NULL,
	[Type] INT NOT NULL CONSTRAINT FK_AddressBookRecord_BusinessType FOREIGN KEY REFERENCES [Lookup].[BusinessType]([Id]),
	[OtherDescription] NVARCHAR(100) NULL,
	[RegistrationNumber] NVARCHAR(64) NOT NULL,
	[AdditionalRegistrationNumber] NVARCHAR(64) NULL,
	[Address1] NVARCHAR(1024) NOT NULL,
	[Address2] NVARCHAR(1024) NULL,
	[TownOrCity] NVARCHAR(1024) NOT NULL,
	[PostalCode] NVARCHAR(64) NULL,
	[Region] NVARCHAR(1024) NULL,
	[Country] NVARCHAR(1024) NOT NULL,
	[CountryId] UNIQUEIDENTIFIER NULL,
	[FirstName] NVARCHAR(1024) NOT NULL,
	[LastName] NVARCHAR(1024) NOT NULL,
	[Telephone] NVARCHAR(150) NOT NULL,
	[Fax] NVARCHAR(150) NULL,
	[Email] NVARCHAR(256) NOT NULL,
	RowVersion ROWVERSION NOT NULL
);
GO

ALTER TABLE [Notification].[Producer]
ADD CONSTRAINT FK_Producer_BusinessType FOREIGN KEY ([Type]) REFERENCES [Lookup].[BusinessType]([Id]);
GO

ALTER TABLE [Notification].[Carrier]
ADD CONSTRAINT FK_Carrier_BusinessType FOREIGN KEY ([Type]) REFERENCES [Lookup].[BusinessType]([Id]);
GO

ALTER TABLE [Notification].[Exporter]
ADD CONSTRAINT FK_Exporter_BusinessType FOREIGN KEY ([Type]) REFERENCES [Lookup].[BusinessType]([Id]);
GO

ALTER TABLE [Notification].[Importer]
ADD CONSTRAINT FK_Importer_BusinessType FOREIGN KEY ([Type]) REFERENCES [Lookup].[BusinessType]([Id]);
GO

ALTER TABLE [Notification].[Facility]
ADD CONSTRAINT FK_Facility_BusinessType FOREIGN KEY ([Type]) REFERENCES [Lookup].[BusinessType]([Id]);
GO