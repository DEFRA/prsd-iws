

GO
PRINT N'Altering [Notification].[Notification]...';


GO
ALTER TABLE [Business].[ShipmentInfo]
    ADD [IsSpecialHandling] BIT NOT NULL DEFAULT 0;


GO
PRINT N'Update complete.';


GO
