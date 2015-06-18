GO
PRINT N'Altering [Notification].[Notification]...';


GO
ALTER TABLE [Notification].[Notification]
    ADD [IsRecoveryPercentageDataProvidedByImporter] BIT             NULL,
        [PercentageRecoverable]                      DECIMAL (18, 2) NULL,
        [MethodOfDisposal]                           NVARCHAR (MAX)  NULL;


GO
PRINT N'Update complete.';


GO