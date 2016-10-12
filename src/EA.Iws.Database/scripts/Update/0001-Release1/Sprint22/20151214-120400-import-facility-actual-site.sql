ALTER TABLE [ImportNotification].[Facility]
ADD [IsActualSiteOfTreatment] BIT NULL;

GO

UPDATE [ImportNotification].[Facility]
SET [IsActualSiteOfTreatment] = 0;

GO

ALTER TABLE [ImportNotification].[Facility]
ALTER COLUMN [IsActualSiteOfTreatment] BIT NOT NULL;

GO