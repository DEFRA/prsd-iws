

GO
PRINT N'Dropping FK_Facility_Country...';


GO
ALTER TABLE [Business].[Facility] DROP CONSTRAINT [FK_Facility_Country];


GO
PRINT N'Dropping FK_NotificationFacility_Facility...';


GO
ALTER TABLE [Notification].[NotificationFacility] DROP CONSTRAINT [FK_NotificationFacility_Facility];


GO
PRINT N'Dropping FK_Notification_Importer...';


GO
ALTER TABLE [Notification].[Notification] DROP CONSTRAINT [FK_Notification_Importer];


GO
PRINT N'Dropping FK_AspNetUsers_Organisation...';


GO
ALTER TABLE [Identity].[AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_Organisation];


GO
PRINT N'Dropping FK_NotificationProducer_Producer...';


GO
ALTER TABLE [Notification].[NotificationProducer] DROP CONSTRAINT [FK_NotificationProducer_Producer];


GO
PRINT N'Dropping FK_Notification_Exporter...';


GO
ALTER TABLE [Notification].[Notification] DROP CONSTRAINT [FK_Notification_Exporter];


GO
PRINT N'Starting rebuilding table [Business].[Facility]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Business].[tmp_ms_xx_Facility] (
    [Id]                           UNIQUEIDENTIFIER NOT NULL,
    [Name]                         NVARCHAR (1024)  NOT NULL,
    [IsActualSiteOfTreatment]      BIT              NOT NULL,
    [Type]                         NVARCHAR (64)    NOT NULL,
    [RegistrationNumber]           NVARCHAR (64)    NOT NULL,
    [AdditionalRegistrationNumber] NVARCHAR (64)    NULL,
    [Building]                     NVARCHAR (1024)  NOT NULL,
    [Address1]                     NVARCHAR (1024)  NOT NULL,
    [Address2]                     NVARCHAR (1024)  NULL,
    [TownOrCity]                   NVARCHAR (1024)  NOT NULL,
    [PostalCode]                   NVARCHAR (64)    NOT NULL,
    [Region]                       NVARCHAR (1024)  NULL,
    [Country]                      NVARCHAR (1024)  NOT NULL,
    [CountryId]                    UNIQUEIDENTIFIER NOT NULL,
    [FirstName]                    NVARCHAR (1024)  NOT NULL,
    [LastName]                     NVARCHAR (1024)  NOT NULL,
    [Telephone]                    NVARCHAR (150)   NOT NULL,
    [Fax]                          NVARCHAR (150)   NULL,
    [Email]                        NVARCHAR (256)   NOT NULL,
    [RowVersion]                   ROWVERSION       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Business].[Facility])
    BEGIN
        INSERT INTO [Business].[tmp_ms_xx_Facility] ([Id], [Name], [IsActualSiteOfTreatment], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Country], [CountryId], [FirstName], [LastName], [Telephone], [Fax], [Email])
        SELECT   [Id],
                 [Name],
                 [IsActualSiteOfTreatment],
                 [Type],
                 [RegistrationNumber],
                 [AdditionalRegistrationNumber],
                 [Building],
                 [Address1],
                 [Address2],
                 [TownOrCity],
                 [PostalCode],
                 [Country],
                 [CountryId],
                 [FirstName],
                 [LastName],
                 [Telephone],
                 [Fax],
                 [Email]
        FROM     [Business].[Facility]
        ORDER BY [Id] ASC;
    END

DROP TABLE [Business].[Facility];

EXECUTE sp_rename N'[Business].[tmp_ms_xx_Facility]', N'Facility';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [Business].[Importer]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Business].[tmp_ms_xx_Importer] (
    [Id]                           UNIQUEIDENTIFIER NOT NULL,
    [Name]                         NVARCHAR (100)   NOT NULL,
    [Type]                         NVARCHAR (64)    NOT NULL,
    [RegistrationNumber]           NVARCHAR (64)    NOT NULL,
    [AdditionalRegistrationNumber] NVARCHAR (64)    NULL,
    [Building]                     NVARCHAR (1024)  NOT NULL,
    [Address1]                     NVARCHAR (1024)  NOT NULL,
    [Address2]                     NVARCHAR (1024)  NULL,
    [TownOrCity]                   NVARCHAR (1024)  NOT NULL,
    [PostalCode]                   NVARCHAR (64)    NOT NULL,
    [Region]                       NVARCHAR (1024)  NULL,
    [Country]                      NVARCHAR (1024)  NOT NULL,
    [CountryId]                    UNIQUEIDENTIFIER NULL,
    [FirstName]                    NVARCHAR (1024)  NOT NULL,
    [LastName]                     NVARCHAR (1024)  NOT NULL,
    [Telephone]                    NVARCHAR (150)   NOT NULL,
    [Fax]                          NVARCHAR (150)   NULL,
    [Email]                        NVARCHAR (256)   NOT NULL,
    [RowVersion]                   ROWVERSION       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Business].[Importer])
    BEGIN
        INSERT INTO [Business].[tmp_ms_xx_Importer] ([Id], [Name], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [Address2], [TownOrCity], [PostalCode], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email])
        SELECT   [Id],
                 [Name],
                 [Type],
                 [RegistrationNumber],
                 [AdditionalRegistrationNumber],
                 [Building],
                 [Address1],
                 [Address2],
                 [TownOrCity],
                 [PostalCode],
                 [Country],
                 [FirstName],
                 [LastName],
                 [Telephone],
                 [Fax],
                 [Email]
        FROM     [Business].[Importer]
        ORDER BY [Id] ASC;
    END

DROP TABLE [Business].[Importer];

EXECUTE sp_rename N'[Business].[tmp_ms_xx_Importer]', N'Importer';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
/*
The column Address1 on table [Business].[Organisation] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Building on table [Business].[Organisation] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Country on table [Business].[Organisation] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column PostalCode on table [Business].[Organisation] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column TownOrCity on table [Business].[Organisation] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [Business].[Organisation]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Business].[tmp_ms_xx_Organisation] (
    [Id]                          UNIQUEIDENTIFIER NOT NULL,
    [Name]                        NVARCHAR (2048)  NOT NULL,
    [Type]                        NVARCHAR (64)    NOT NULL,
    [RowVersion]                  ROWVERSION       NOT NULL,
    [RegistrationNumber]          NVARCHAR (64)    NULL,
    [AditionalRegistrationNumber] NVARCHAR (64)    NULL,
    [Building]                    NVARCHAR (1024)  NOT NULL,
    [Address1]                    NVARCHAR (1024)  NOT NULL,
    [TownOrCity]                  NVARCHAR (1024)  NOT NULL,
    [Address2]                    NVARCHAR (1024)  NULL,
    [Region]                      NVARCHAR (1024)  NULL,
    [PostalCode]                  NVARCHAR (64)    NOT NULL,
    [Country]                     NVARCHAR (1024)  NOT NULL,
    [FirstName]                   NVARCHAR (150)   NULL,
    [LastName]                    NVARCHAR (150)   NULL,
    [Telephone]                   NVARCHAR (150)   NULL,
    [Fax]                         NVARCHAR (150)   NULL,
    [Email]                       NVARCHAR (150)   NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Organisation_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Business].[Organisation])
    BEGIN
        INSERT INTO [Business].[tmp_ms_xx_Organisation] ([Id], [Name], [Type], [RegistrationNumber], [AditionalRegistrationNumber], [Building], [Address1], [TownOrCity], [Address2], [PostalCode], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email])
        SELECT   [Id],
                 [Name],
                 [Type],
                 [RegistrationNumber],
                 [AditionalRegistrationNumber],
                 [Building],
                 [Address1],
                 [TownOrCity],
                 [Address2],
                 [PostalCode],
                 [Country],
                 [FirstName],
                 [LastName],
                 [Telephone],
                 [Fax],
                 [Email]
        FROM     [Business].[Organisation]
        ORDER BY [Id] ASC;
    END

DROP TABLE [Business].[Organisation];

EXECUTE sp_rename N'[Business].[tmp_ms_xx_Organisation]', N'Organisation';

EXECUTE sp_rename N'[Business].[tmp_ms_xx_constraint_PK_Organisation_Id]', N'PK_Organisation_Id', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
/*
The column Address1 on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Building on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Country on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Email on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column FirstName on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column LastName on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column PostalCode on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column RegistrationNumber on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Telephone on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column TownOrCity on table [Business].[Producer] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [Business].[Producer]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Business].[tmp_ms_xx_Producer] (
    [Id]                           UNIQUEIDENTIFIER NOT NULL,
    [Name]                         NVARCHAR (100)   NOT NULL,
    [IsSiteOfExport]               BIT              NOT NULL,
    [Type]                         NVARCHAR (64)    NOT NULL,
    [RegistrationNumber]           NVARCHAR (64)    NOT NULL,
    [AdditionalRegistrationNumber] NVARCHAR (64)    NULL,
    [Building]                     NVARCHAR (1024)  NOT NULL,
    [Address1]                     NVARCHAR (1024)  NOT NULL,
    [Address2]                     NVARCHAR (1024)  NULL,
    [TownOrCity]                   NVARCHAR (1024)  NOT NULL,
    [PostalCode]                   NVARCHAR (64)    NOT NULL,
    [Region]                       NVARCHAR (1024)  NULL,
    [Country]                      NVARCHAR (1024)  NOT NULL,
    [CountryId]                    UNIQUEIDENTIFIER NULL,
    [FirstName]                    NVARCHAR (1024)  NOT NULL,
    [LastName]                     NVARCHAR (1024)  NOT NULL,
    [Telephone]                    NVARCHAR (150)   NOT NULL,
    [Fax]                          NVARCHAR (150)   NULL,
    [Email]                        NVARCHAR (256)   NOT NULL,
    [RowVersion]                   ROWVERSION       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Business].[Producer])
    BEGIN
        INSERT INTO [Business].[tmp_ms_xx_Producer] ([Id], [Name], [IsSiteOfExport], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [TownOrCity], [Address2], [PostalCode], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email])
        SELECT   [Id],
                 [Name],
                 [IsSiteOfExport],
                 [Type],
                 [RegistrationNumber],
                 [AdditionalRegistrationNumber],
                 [Building],
                 [Address1],
                 [TownOrCity],
                 [Address2],
                 [PostalCode],
                 [Country],
                 [FirstName],
                 [LastName],
                 [Telephone],
                 [Fax],
                 [Email]
        FROM     [Business].[Producer]
        ORDER BY [Id] ASC;
    END

DROP TABLE [Business].[Producer];

EXECUTE sp_rename N'[Business].[tmp_ms_xx_Producer]', N'Producer';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
/*
The column Address1 on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Building on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Country on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Email on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column FirstName on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column LastName on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column PostalCode on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column RegistrationNumber on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Telephone on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column TownOrCity on table [Notification].[Exporter] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [Notification].[Exporter]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Business].[tmp_ms_xx_Exporter] (
    [Id]                           UNIQUEIDENTIFIER NOT NULL,
    [Name]                         NVARCHAR (20)    NOT NULL,
    [Type]                         NVARCHAR (64)    NOT NULL,
    [RegistrationNumber]           NVARCHAR (64)    NOT NULL,
    [AdditionalRegistrationNumber] NVARCHAR (64)    NULL,
    [Building]                     NVARCHAR (1024)  NOT NULL,
    [Address1]                     NVARCHAR (1024)  NOT NULL,
    [Address2]                     NVARCHAR (1024)  NULL,
    [TownOrCity]                   NVARCHAR (1024)  NOT NULL,
    [PostalCode]                   NVARCHAR (64)    NOT NULL,
    [Region]                       NVARCHAR (1024)  NULL,
    [Country]                      NVARCHAR (1024)  NOT NULL,
    [CountryId]                    UNIQUEIDENTIFIER NULL,
    [FirstName]                    NVARCHAR (1024)  NOT NULL,
    [LastName]                     NVARCHAR (1024)  NOT NULL,
    [Telephone]                    NVARCHAR (150)   NOT NULL,
    [Fax]                          NVARCHAR (150)   NULL,
    [Email]                        NVARCHAR (256)   NOT NULL,
    [RowVersion]                   ROWVERSION       NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Exporter] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Business].[Exporter])
    BEGIN
        INSERT INTO [Business].[tmp_ms_xx_Exporter] ([Id], [Name], [Type], [RegistrationNumber], [AdditionalRegistrationNumber], [Building], [Address1], [TownOrCity], [Address2], [PostalCode], [Country], [FirstName], [LastName], [Telephone], [Fax], [Email])
        SELECT   [Id],
                 [Name],
                 [Type],
                 [RegistrationNumber],
                 [AdditionalRegistrationNumber],
                 [Building],
                 [Address1],
                 [TownOrCity],
                 [Address2],
                 [PostalCode],
                 [Country],
                 [FirstName],
                 [LastName],
                 [Telephone],
                 [Fax],
                 [Email]
        FROM     [Business].[Exporter]
        ORDER BY [Id] ASC;
    END

DROP TABLE [Business].[Exporter];

EXECUTE sp_rename N'[Business].[tmp_ms_xx_Exporter]', N'Exporter';

EXECUTE sp_rename N'[Business].[tmp_ms_xx_constraint_PK_Exporter]', N'PK_Exporter', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating FK_Facility_Country...';


GO
ALTER TABLE [Business].[Facility] WITH NOCHECK
    ADD CONSTRAINT [FK_Facility_Country] FOREIGN KEY ([CountryId]) REFERENCES [Lookup].[Country] ([Id]);


GO
PRINT N'Creating FK_NotificationFacility_Facility...';


GO
ALTER TABLE [Notification].[NotificationFacility] WITH NOCHECK
    ADD CONSTRAINT [FK_NotificationFacility_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [Business].[Facility] ([Id]);


GO
PRINT N'Creating FK_Notification_Importer...';


GO
ALTER TABLE [Notification].[Notification] WITH NOCHECK
    ADD CONSTRAINT [FK_Notification_Importer] FOREIGN KEY ([ImporterId]) REFERENCES [Business].[Importer] ([Id]);


GO
PRINT N'Creating FK_AspNetUsers_Organisation...';


GO
ALTER TABLE [Identity].[AspNetUsers] WITH NOCHECK
    ADD CONSTRAINT [FK_AspNetUsers_Organisation] FOREIGN KEY ([OrganisationId]) REFERENCES [Business].[Organisation] ([Id]);


GO
PRINT N'Creating FK_NotificationProducer_Producer...';


GO
ALTER TABLE [Notification].[NotificationProducer] WITH NOCHECK
    ADD CONSTRAINT [FK_NotificationProducer_Producer] FOREIGN KEY ([ProducerId]) REFERENCES [Business].[Producer] ([Id]);


GO
PRINT N'Creating FK_Notification_Exporter...';


GO
ALTER TABLE [Notification].[Notification] WITH NOCHECK
    ADD CONSTRAINT [FK_Notification_Exporter] FOREIGN KEY ([ExporterId]) REFERENCES [Business].[Exporter] ([Id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO


GO
ALTER TABLE [Business].[Facility] WITH CHECK CHECK CONSTRAINT [FK_Facility_Country];

ALTER TABLE [Notification].[NotificationFacility] WITH CHECK CHECK CONSTRAINT [FK_NotificationFacility_Facility];

ALTER TABLE [Notification].[Notification] WITH CHECK CHECK CONSTRAINT [FK_Notification_Importer];

ALTER TABLE [Identity].[AspNetUsers] WITH CHECK CHECK CONSTRAINT [FK_AspNetUsers_Organisation];

ALTER TABLE [Notification].[NotificationProducer] WITH CHECK CHECK CONSTRAINT [FK_NotificationProducer_Producer];

ALTER TABLE [Notification].[Notification] WITH CHECK CHECK CONSTRAINT [FK_Notification_Exporter];


GO
PRINT N'Update complete.';


GO
