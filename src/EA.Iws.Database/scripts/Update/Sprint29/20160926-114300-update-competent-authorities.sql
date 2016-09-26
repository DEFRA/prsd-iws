-- Update CAs
UPDATE [Lookup].[CompetentAuthority]
SET Code = 'CZ15'
WHERE Code = 'CA';

UPDATE [Lookup].[CompetentAuthority]
SET Code = 'DK001'
WHERE Code = 'DK';

UPDATE [Lookup].[CompetentAuthority]
SET Code = 'EE-001'
WHERE Code = 'EE';

UPDATE [Lookup].[CompetentAuthority]
SET Code = 'FI001'
WHERE Code = 'FIN';

UPDATE [Lookup].[CompetentAuthority]
SET Code = 'GR001'
WHERE Code = 'GR';

UPDATE [Lookup].[CompetentAuthority]
SET Code = 'LV001'
WHERE Code = 'LV';

UPDATE [Lookup].[CompetentAuthority]
SET Code = 'LU001'
WHERE Code = 'LU';

UPDATE [Lookup].[CompetentAuthority]
SET Code = 'MT 00'
WHERE Code = 'MT';

UPDATE [Lookup].[CompetentAuthority]
SET Code = 'NL 001',
	Name = 'ILT Inspectie Leefomgeving en Transport (EVOA)'
WHERE Code = 'NL';

UPDATE [Lookup].[Country]
SET Name = 'Democratic People''s Republic of Korea'
WHERE Name = 'North Korea';

UPDATE [Lookup].[Country]
SET Name = 'Republic of Korea'
WHERE Name = 'South Korea';

-- Insert CAs
IF NOT EXISTS (SELECT Id FROM [Lookup].[CompetentAuthority] WHERE Code = 'MT 01')
BEGIN
	INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId], [Region])
	SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
			'Malta Environment and Planning Authority',
			'MT 01',
			(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Malta'),
			'Competent Authority of dispatch'
END;

IF NOT EXISTS (SELECT Id FROM [Lookup].[CompetentAuthority] WHERE Code = 'MT 10')
BEGIN
	INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId], [Region])
	SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
			'Malta Environment and Planning Authority',
			'MT 10',
			(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Malta'),
			'Competent Authority of destination'
END;

DELETE FROM [Lookup].[CompetentAuthority]
WHERE Code in ('KP', 'KR');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Ministry of Land and Environment Protection',
		'KP',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Democratic People''s Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'National Coordinating Committee for Environment (NCCE)',
		'KP',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Democratic People''s Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Saemangeum Regional Environmental Office',
		'KR',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Nakdong River Basin Environmental Office',
		'KR',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Geum River Basin Environmental Office',
		'KR',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Wonju Regional Environmental Office',
		'KR',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Daegu Regional Environmental Office',
		'KR',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Yeongsan River Basin Environmental Office',
		'KR',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Han River Basin Environmental Office',
		'KR',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Republic of Korea');

INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		'Ministry of Environment',
		'KR',
		(SELECT Id FROM [Lookup].[Country] WHERE Name = 'Republic of Korea');

-- Insert 'Other' entries for countries without a CA
INSERT INTO [Lookup].[CompetentAuthority] ([Id], [Name], [Code], [CountryId])
SELECT		(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
			'Other',
			C.[IsoAlpha2Code],
			C.[Id]
FROM		[Lookup].[Country] C
WHERE		C.Id NOT IN (
	SELECT CountryId FROM [Lookup].[CompetentAuthority] 
)