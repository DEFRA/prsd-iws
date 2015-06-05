PRINT 'Create Entry or Exit Points table'

CREATE TABLE [Lookup].[EntryOrExitPoint]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_EntryOrExitPoint PRIMARY KEY CONSTRAINT DF_EntryOrExitPoint_Id DEFAULT NEWSEQUENTIALID(),
	[Name] NVARCHAR(2048),
	[CountryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_EntryOrExitPoint_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id])
);
GO

PRINT 'Deleting existing data from Competent Authority table'

IF (SELECT COUNT(1) FROM [Lookup].CompetentAuthority) > 0
BEGIN
	DELETE FROM [Lookup].[CompetentAuthority];
END
GO

PRINT 'Add default for Id from Competent Authority table'

ALTER TABLE [Lookup].[CompetentAuthority]
ADD CONSTRAINT DF_CompetentAuthority_Id DEFAULT NEWSEQUENTIALID() FOR [Id];
GO

PRINT 'Adding the code column to Competent Authority table'

ALTER TABLE [Lookup].[CompetentAuthority]
ADD [Code] NVARCHAR(25) NOT NULL;
GO

PRINT 'Adding country column to Competent Authority table'

ALTER TABLE [Lookup].[CompetentAuthority]
ADD [CountryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_CompetentAuthority_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id]);
GO

PRINT 'Removing row version from Competent Authority column'

ALTER TABLE [Lookup].[CompetentAuthority]
DROP COLUMN [RowVersion];
GO

PRINT 'Creating Transport Route tables'

CREATE TABLE [Notification].[StateOfExport]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_StateOfExport PRIMARY KEY,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_StateOfExport_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id]),
	[CountryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_StateOfExport_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[CompetentAuthorityId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_StateOfExport_CompetentAuthority FOREIGN KEY REFERENCES [Lookup].[CompetentAuthority]([Id]),
	[ExitPointId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_StateOfExport_ExitPoint FOREIGN KEY REFERENCES [Lookup].[EntryOrExitPoint]([Id]),
	[RowVersion] ROWVERSION NOT NULL
)
GO

CREATE TABLE [Notification].[StateOfImport]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_StateOfImport PRIMARY KEY,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_StateOfImport_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id]),
	[CountryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_StateOfImport_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[CompetentAuthorityId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_StateOfImport_CompetentAuthority FOREIGN KEY REFERENCES [Lookup].[CompetentAuthority]([Id]),
	[EntryPointId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_StateOfImport_EntryPoint FOREIGN KEY REFERENCES [Lookup].[EntryOrExitPoint]([Id]),
	[RowVersion] ROWVERSION NOT NULL
)
GO

CREATE TABLE [Notification].[TransitState]
(
	[Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_TransitState PRIMARY KEY,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_TransitState_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id]),
	[CountryId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_TransitState_Country FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[CompetentAuthorityId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_TransitState_CompetentAuthority FOREIGN KEY REFERENCES [Lookup].[CompetentAuthority]([Id]),
	[EntryPointId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_TransitState_EntryPoint FOREIGN KEY REFERENCES [Lookup].[EntryOrExitPoint]([Id]),
	[ExitPointId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_TransitState_ExitPoint FOREIGN KEY REFERENCES [Lookup].[EntryOrExitPoint]([Id]),
	[RowVersion] ROWVERSION NOT NULL
)
GO