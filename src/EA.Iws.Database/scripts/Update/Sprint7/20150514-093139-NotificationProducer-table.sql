

GO
PRINT N'Creating [Business].[NotificationProducer]...';


GO
CREATE TABLE [Business].[NotificationProducer] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [ProducerId]     UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [Identity].[Consents]...';


GO
CREATE TABLE [Identity].[Consents] (
    [Subject]  NVARCHAR (200)  NOT NULL,
    [ClientId] NVARCHAR (200)  NOT NULL,
    [Scopes]   NVARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_Identity.Consents] PRIMARY KEY CLUSTERED ([Subject] ASC, [ClientId] ASC)
);


GO
PRINT N'Creating [Identity].[Tokens]...';


GO
CREATE TABLE [Identity].[Tokens] (
    [Key]       NVARCHAR (128)     NOT NULL,
    [TokenType] SMALLINT           NOT NULL,
    [SubjectId] NVARCHAR (200)     NULL,
    [ClientId]  NVARCHAR (200)     NOT NULL,
    [JsonCode]  NVARCHAR (MAX)     NOT NULL,
    [Expiry]    DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_Identity.Tokens] PRIMARY KEY CLUSTERED ([Key] ASC, [TokenType] ASC)
);


GO
PRINT N'Creating DF_NotificationProducerId...';


GO
ALTER TABLE [Business].[NotificationProducer]
    ADD CONSTRAINT [DF_NotificationProducerId] DEFAULT (newsequentialid()) FOR [Id];


GO
PRINT N'Creating FK_NotificationProducer_Producer...';


GO
ALTER TABLE [Business].[NotificationProducer] WITH NOCHECK
    ADD CONSTRAINT [FK_NotificationProducer_Producer] FOREIGN KEY ([ProducerId]) REFERENCES [Business].[Producer] ([Id]);


GO
PRINT N'Creating FK_NotificationProducer_Notification...';


GO
ALTER TABLE [Business].[NotificationProducer] WITH NOCHECK
    ADD CONSTRAINT [FK_NotificationProducer_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] ([Id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO


GO
ALTER TABLE [Business].[NotificationProducer] WITH CHECK CHECK CONSTRAINT [FK_NotificationProducer_Producer];

ALTER TABLE [Business].[NotificationProducer] WITH CHECK CHECK CONSTRAINT [FK_NotificationProducer_Notification];


GO
PRINT N'Update complete.';


GO
