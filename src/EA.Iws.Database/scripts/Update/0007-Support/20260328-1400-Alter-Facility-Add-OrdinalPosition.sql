/* =========================================================
   Notification.Facility
   ========================================================= */

IF COL_LENGTH('Notification.Facility', 'OrdinalPosition') IS NULL
BEGIN
    ALTER TABLE [Notification].[Facility]
    ADD [OrdinalPosition] INT NULL;
END
GO

WITH OrderedFacilities AS
(
    SELECT
        F.Id,
        ROW_NUMBER() OVER
        (
            PARTITION BY F.FacilityCollectionId
            ORDER BY F.RowVersion
        ) AS RowNum
    FROM [Notification].[Facility] F
)
UPDATE F
SET F.[OrdinalPosition] = OFC.RowNum
FROM [Notification].[Facility] F
INNER JOIN OrderedFacilities OFC
    ON F.Id = OFC.Id
WHERE F.[OrdinalPosition] IS NULL;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t
        ON t.object_id = c.object_id
    INNER JOIN sys.schemas s
        ON s.schema_id = t.schema_id
    WHERE s.name = 'Notification'
      AND t.name = 'Facility'
      AND c.name = 'OrdinalPosition'
)
BEGIN
    ALTER TABLE [Notification].[Facility]
    ADD CONSTRAINT [DF_Notification_Facility_OrdinalPosition]
    DEFAULT (1) FOR [OrdinalPosition];
END
GO

IF EXISTS
(
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t
        ON t.object_id = c.object_id
    INNER JOIN sys.schemas s
        ON s.schema_id = t.schema_id
    WHERE s.name = 'Notification'
      AND t.name = 'Facility'
      AND c.name = 'OrdinalPosition'
      AND c.is_nullable = 1
)
BEGIN
    ALTER TABLE [Notification].[Facility]
    ALTER COLUMN [OrdinalPosition] INT NOT NULL;
END
GO


/* =========================================================
   ImportNotification.Facility
   ========================================================= */

IF COL_LENGTH('ImportNotification.Facility', 'OrdinalPosition') IS NULL
BEGIN
    ALTER TABLE [ImportNotification].[Facility]
    ADD [OrdinalPosition] INT NULL;
END
GO

WITH OrderedFacilities AS
(
    SELECT
        F.Id,
        ROW_NUMBER() OVER
        (
            PARTITION BY F.FacilityCollectionId
            ORDER BY F.RowVersion
        ) AS RowNum
    FROM [ImportNotification].[Facility] F
)
UPDATE F
SET F.[OrdinalPosition] = OFC.RowNum
FROM [ImportNotification].[Facility] F
INNER JOIN OrderedFacilities OFC
    ON F.Id = OFC.Id
WHERE F.[OrdinalPosition] IS NULL;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.default_object_id = dc.object_id
    INNER JOIN sys.tables t
        ON t.object_id = c.object_id
    INNER JOIN sys.schemas s
        ON s.schema_id = t.schema_id
    WHERE s.name = 'ImportNotification'
      AND t.name = 'Facility'
      AND c.name = 'OrdinalPosition'
)
BEGIN
    ALTER TABLE [ImportNotification].[Facility]
    ADD CONSTRAINT [DF_ImportNotification_Facility_OrdinalPosition]
    DEFAULT (1) FOR [OrdinalPosition];
END
GO

IF EXISTS
(
    SELECT 1
    FROM sys.columns c
    INNER JOIN sys.tables t
        ON t.object_id = c.object_id
    INNER JOIN sys.schemas s
        ON s.schema_id = t.schema_id
    WHERE s.name = 'ImportNotification'
      AND t.name = 'Facility'
      AND c.name = 'OrdinalPosition'
      AND c.is_nullable = 1
)
BEGIN
    ALTER TABLE [ImportNotification].[Facility]
    ALTER COLUMN [OrdinalPosition] INT NOT NULL;
END
GO