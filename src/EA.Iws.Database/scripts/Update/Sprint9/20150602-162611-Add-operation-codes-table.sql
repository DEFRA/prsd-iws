

GO
PRINT N'Creating [Business].[OperationCodes]...';


GO
CREATE TABLE [Business].[OperationCodes] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL,
    [OperationCode]   INT              NOT NULL,
    [RowVersion]     ROWVERSION       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating FK_OperationCodes_Notification...';


GO
ALTER TABLE [Business].[OperationCodes] WITH NOCHECK
    ADD CONSTRAINT [FK_OperationCodes_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] ([Id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO


GO
ALTER TABLE [Business].[OperationCodes] WITH CHECK CHECK CONSTRAINT [FK_OperationCodes_Notification];


GO
PRINT N'Update complete.';


GO
