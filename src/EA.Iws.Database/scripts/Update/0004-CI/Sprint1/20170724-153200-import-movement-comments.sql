-- Migrate FurtherDetails into Reason and delete FurtherDetails

ALTER TABLE [ImportNotification].[MovementRejection]
ALTER COLUMN [Reason] NVARCHAR(MAX) NOT NULL;
GO

UPDATE [ImportNotification].[MovementRejection]
SET [Reason] = [Reason] + CHAR(13) + CHAR(10) + [FurtherDetails]
WHERE [FurtherDetails] IS NOT NULL;
GO

ALTER TABLE [ImportNotification].[MovementRejection]
DROP COLUMN [FurtherDetails];
GO

-- Add Comments and stats marking columns

ALTER TABLE [ImportNotification].[Movement]
ADD [Comments] NVARCHAR(MAX) NULL,
    [StatsMarking] NVARCHAR(1024) NULL;
GO