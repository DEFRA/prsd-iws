IF OBJECT_ID('[Business].[Facility]') IS NOT NULL
BEGIN
	ALTER TABLE [Business].[Facility]
	DROP CONSTRAINT FK_Facility_Country;
	
	ALTER TABLE [Business].[Facility]
	ALTER COLUMN [CountryId] UNIQUEIDENTIFIER NULL;
END;
GO