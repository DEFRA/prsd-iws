DECLARE @CountryId UNIQUEIDENTIFIER;
DECLARE @IsoAlpha2Code NCHAR(2);

SET @CountryId = (SELECT Id FROM [Lookup].[Country] WHERE [Name] = 'Spain');
SET @IsoAlpha2Code = (SELECT IsoAlpha2Code FROM [Lookup].[Country] WHERE [Name] = 'Spain');

UPDATE
		[Lookup].[CompetentAuthority]
	SET
		[IsTransitAuthority] = 0
	WHERE
		[CountryId] = @CountryId AND [Code] = @IsoAlpha2Code;

INSERT INTO [Lookup].[CompetentAuthority]
			([Id]
			,[Name]
			,[Abbreviation]
			,[IsSystemUser]
			,[Code]
			,[CountryId]
			,[Region]
			,[IsTransitAuthority])
	VALUES
			('AD8819E4-6B05-46EC-8CAA-F3052CF7D1E1'
			,'Ministerio  para la Transicion Ecologica y el Reto Demografico (Ministry for Ecological Transition and Demographic Challenge)'
			,NULL
			,0
			,@IsoAlpha2Code
			,@CountryId
			,NULL
			,1);
