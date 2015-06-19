GO
PRINT N'Creating [Business].[Carrier]...';


GO
CREATE TABLE [Business].[Carrier] (
    [Id]                           UNIQUEIDENTIFIER NOT NULL,
    [Name]                         NVARCHAR (3000)   NOT NULL,
	[NotificationId]			   UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_Carrier_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id]),
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


GO
PRINT N'Update complete.';


GO
