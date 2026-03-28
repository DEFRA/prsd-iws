ALTER TABLE [Notification].[Facility]
ADD [OrdinalPosition] INT NULL;

GO
WITH OrderedFacilities AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY FacilityCollectionId ORDER BY RowVersion) AS RowNum
    FROM [Notification].[Facility]
)
UPDATE [Notification].[Facility]
SET [OrdinalPosition] = OrderedFacilities.RowNum
FROM OrderedFacilities
WHERE [Notification].[Facility].Id = OrderedFacilities.Id;

GO
ALTER TABLE [Notification].[Facility]
ALTER COLUMN [OrdinalPosition] INT NOT NULL;

GO
ALTER TABLE [ImportNotification].[Facility]
ADD [OrdinalPosition] INT NULL;

GO
WITH OrderedFacilities AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY FacilityCollectionId ORDER BY RowVersion) AS RowNum
    FROM [ImportNotification].[Facility]
)
UPDATE [ImportNotification].[Facility]
SET [OrdinalPosition] = OrderedFacilities.RowNum
FROM OrderedFacilities
WHERE [ImportNotification].[Facility].Id = OrderedFacilities.Id;

GO
ALTER TABLE [ImportNotification].[Facility]
ALTER COLUMN [OrdinalPosition] INT NOT NULL;

GO