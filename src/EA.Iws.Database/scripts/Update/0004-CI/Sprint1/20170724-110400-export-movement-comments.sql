-- Migrate FurtherDetails into Reason and delete FurtherDetails

ALTER TABLE [Notification].[MovementRejection]
ALTER COLUMN [Reason] NVARCHAR(MAX) NOT NULL;
GO

UPDATE [Notification].[MovementRejection]
SET [Reason] = [Reason] + CHAR(13) + CHAR(10) + [FurtherDetails]
WHERE [FurtherDetails] IS NOT NULL;
GO

ALTER TABLE [Notification].[MovementRejection]
DROP COLUMN [FurtherDetails];
GO

-- Add Comments and stats marking columns

ALTER TABLE [Notification].[Movement]
ADD [Comments] NVARCHAR(MAX) NULL,
    [StatsMarking] NVARCHAR(1024) NULL;
GO