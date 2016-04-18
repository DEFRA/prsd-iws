

GO
PRINT N'Altering [Notification].[Notification]...';


GO
ALTER TABLE [Business].[ShipmentInfo]
    ADD [IsSpecialHandling] BIT NOT NULL DEFAULT 0;


GO
ALTER TABLE [Business].[ShipmentInfo]
    ADD [SpecialHandlingDetails] NVARCHAR (2048) NULL;


GO
PRINT N'Update complete.';


GO
