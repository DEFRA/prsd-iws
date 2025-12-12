DECLARE @IsoAlpha2Code NCHAR(2);

SET @IsoAlpha2Code = 'MK';

UPDATE
		[Lookup].[Country]
	SET
		[Name] = 'North Macedonia'
	WHERE
		[IsoAlpha2Code] = @IsoAlpha2Code AND [Name] = 'Macedonia';

GO