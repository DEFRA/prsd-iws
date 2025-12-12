DECLARE @CountryId UNIQUEIDENTIFIER;
DECLARE @IsoAlpha2Code NCHAR(2);

SET @CountryId = (SELECT Id FROM [Lookup].[Country] WHERE [IsoAlpha2Code] = 'MK');
SET @IsoAlpha2Code = (SELECT IsoAlpha2Code FROM [Lookup].[Country] WHERE [IsoAlpha2Code] = 'MK');

UPDATE
		[Lookup].[Country]
	SET
		[Name] = 'North Macedonia'
	WHERE
		[IsoAlpha2Code] = @IsoAlpha2Code AND [Name] = 'Macedonia';

GO
PRINT N'Macedonia updated to North Macedonia successfully.';
GO