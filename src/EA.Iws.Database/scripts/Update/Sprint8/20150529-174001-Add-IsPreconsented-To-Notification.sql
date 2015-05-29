GO
PRINT N'Starting rebuilding table [Notification].[Notification]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [Notification].[tmp_ms_xx_Notification] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NOT NULL,
    [NotificationType]   INT              NOT NULL,
    [CompetentAuthority] INT              NOT NULL,
    [NotificationNumber] NVARCHAR (50)    NOT NULL,
    [IsPreconsentedRecoveryFacility]     BIT              NULL,
    [RowVersion]         ROWVERSION       NOT NULL,
    [CreatedDate]        DATETIME2 (0)    CONSTRAINT [DF_Notification_CreatedDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Notification] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [Notification].[Notification])
    BEGIN
        INSERT INTO [Notification].[tmp_ms_xx_Notification] ([Id], [UserId], [NotificationType], [CompetentAuthority], [NotificationNumber], [CreatedDate])
        SELECT   [Id],
                 [UserId],
                 [NotificationType],
                 [CompetentAuthority],
                 [NotificationNumber],
                 [CreatedDate]
        FROM     [Notification].[Notification]
        ORDER BY [Id] ASC;
    END

DROP TABLE [Notification].[Notification];

EXECUTE sp_rename N'[Notification].[tmp_ms_xx_Notification]', N'Notification';

EXECUTE sp_rename N'[Notification].[tmp_ms_xx_constraint_PK_Notification]', N'PK_Notification', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Update complete.';


GO
