

GO
PRINT N'Altering [Notification].[Notification]...';


GO
ALTER TABLE [Notification].[Notification]
    ADD [IsSpecialHandling] BIT NOT NULL DEFAULT 0;


GO
PRINT N'Update complete.';


GO
