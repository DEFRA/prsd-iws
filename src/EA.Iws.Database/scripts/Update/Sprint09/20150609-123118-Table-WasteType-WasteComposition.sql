GO
PRINT N'Creating [Business].[WasteComposition]...';


GO
CREATE TABLE [Business].[WasteComposition] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [Constituent]      NVARCHAR (256)   NOT NULL,
    [MinConcentration] DECIMAL (5, 2)   NOT NULL,
    [MaxConcentration] DECIMAL (5, 2)   NOT NULL,
    [RowVersion]       ROWVERSION       NOT NULL,
    [WasteTypeId]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_WasteComposition] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [Business].[WasteType]...';


GO
CREATE TABLE [Business].[WasteType] (
    [Id]                             UNIQUEIDENTIFIER NOT NULL,
    [ChemicalCompositionType]        INT              NOT NULL,
    [ChemicalCompositionName]        NVARCHAR (120)   NULL,
    [ChemicalCompositionDescription] NVARCHAR (MAX)   NULL,
    [RowVersion]                     ROWVERSION       NOT NULL,
    [NotificationId]                 UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_WasteType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating FK_WasteComposition_WasteType...';


GO
ALTER TABLE [Business].[WasteComposition] WITH NOCHECK
    ADD CONSTRAINT [FK_WasteComposition_WasteType] FOREIGN KEY ([WasteTypeId]) REFERENCES [Business].[WasteType] ([Id]);


GO
PRINT N'Creating FK_WasteType_Notification...';


GO
ALTER TABLE [Business].[WasteType] WITH NOCHECK
    ADD CONSTRAINT [FK_WasteType_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] ([Id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [Business].[WasteComposition] WITH CHECK CHECK CONSTRAINT [FK_WasteComposition_WasteType];

ALTER TABLE [Business].[WasteType] WITH CHECK CHECK CONSTRAINT [FK_WasteType_Notification];


GO
PRINT N'Update complete.';


GO
