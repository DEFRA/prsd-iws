CREATE SCHEMA [Person] AUTHORIZATION [dbo];
GO

CREATE TABLE [Person].[InternalUser] (
    [Id]					UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_InternalUser PRIMARY KEY,
    [UserId]				NVARCHAR(128) NOT NULL CONSTRAINT FK_InternalUser_AspNetUsers FOREIGN KEY REFERENCES [Identity].[AspNetUsers](Id),
    [RowVersion]            [timestamp] NOT NULL,
    [JobTitle]				NVARCHAR(256) NOT NULL,
    [CompetentAuthority]	INT NOT NULL,
    [LocalAreaId]			UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_InternalUser_LocalArea FOREIGN KEY REFERENCES [Lookup].[LocalArea](Id),
    [Status]				INT NOT NULL
);

GO

DECLARE @headOffice UNIQUEIDENTIFIER;
DECLARE @NRW UNIQUEIDENTIFIER;
DECLARE @NIEA UNIQUEIDENTIFIER;
DECLARE @SEPA UNIQUEIDENTIFIER;

SELECT @headOffice = Id FROM [Lookup].[LocalArea] WHERE Name = 'Head Office';
SELECT @NRW = Id FROM [Lookup].[LocalArea] WHERE Name = 'Natural Resources Wales';
SELECT @NIEA = Id FROM [Lookup].[LocalArea] WHERE Name = 'Northern Ireland Environment Agency';
SELECT @SEPA = Id FROM [Lookup].[LocalArea] WHERE Name = 'Scottish Environment Protection Agency';

INSERT INTO [Person].[InternalUser] (
    [Id],	
    [UserId],
    [JobTitle],
    [CompetentAuthority],
    [LocalAreaId],
    [Status]	
)
SELECT
    (select CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)) as [Id],
    U.[Id],
    [JobTitle],
    CASE [CompetentAuthority] WHEN 'EA' THEN 1
        WHEN 'SEPA' THEN 2
        WHEN 'NIEA' THEN 3
        ELSE 4
        END,
    CASE WHEN LA.Id IS NULL THEN 
        CASE [CompetentAuthority] WHEN 'EA' THEN @headOffice
        WHEN 'SEPA' THEN @SEPA
        WHEN 'NIEA' THEN @NIEA
        ELSE @NRW
        END ELSE LA.Id END,
    [InternalUserStatus]
FROM
    [Identity].[AspNetUsers] U
    LEFT JOIN [Lookup].[LocalArea] LA ON U.LocalArea = LA.Name
WHERE
    IsInternal = 1;

GO

ALTER TABLE [Identity].[AspNetUsers]
DROP CONSTRAINT [DF_AspNetUsers_IsInternal]

ALTER TABLE [Identity].[AspNetUsers]
DROP COLUMN [JobTitle]

ALTER TABLE [Identity].[AspNetUsers]
DROP COLUMN [CompetentAuthority]

ALTER TABLE [Identity].[AspNetUsers]
DROP COLUMN [LocalArea]

ALTER TABLE [Identity].[AspNetUsers]
DROP COLUMN [IsInternal]

ALTER TABLE [Identity].[AspNetUsers]
DROP COLUMN [InternalUserStatus]