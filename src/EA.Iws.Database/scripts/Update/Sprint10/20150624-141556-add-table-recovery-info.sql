
GO
PRINT N'Creating [Business].[RecoveryInfo]...';


GO
CREATE TABLE [Business].[RecoveryInfo] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [NotificationId]  UNIQUEIDENTIFIER NOT NULL,
    [EstimatedUnit]   INT              NOT NULL,
    [EstimatedAmount] DECIMAL (12, 2)  NOT NULL,
    [CostUnit]        INT              NOT NULL,
    [CostAmount]      DECIMAL (12, 2)  NOT NULL,
    [DisposalUnit]    INT              NULL,
    [DisposalAmount]  DECIMAL (12, 2)  NULL,
    [RowVersion]      ROWVERSION       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
PRINT N'Creating FK_RecoveryInfo_Notification...';

GO
ALTER TABLE [Business].[RecoveryInfo] WITH NOCHECK
    ADD CONSTRAINT [FK_RecoveryInfo_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] ([Id]);

GO
ALTER TABLE [Business].[RecoveryInfo] WITH CHECK CHECK CONSTRAINT [FK_RecoveryInfo_Notification];

GO
PRINT N'Update complete.';

GO