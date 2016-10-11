UPDATE [Lookup].[CompetentAuthority]
SET Name = 'Ministère de l''écologie, du développement durable et de l''énergie'
WHERE Abbreviation = 'DGPR/SPNQE'

UPDATE [Lookup].[EntryOrExitPoint]
SET Name = 'Southampton'
WHERE Name = 'Southhampton'

DECLARE @HOLLAND UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM [Lookup].[Country] WHERE Name = 'The Netherlands')

INSERT INTO [Lookup].[CompetentAuthority] (Name, Abbreviation, IsSystemUser, Code, CountryId)
VALUES ('Inspectie Leefomgeving en Transport', 'ILT', 0, 'NL0001', @HOLLAND)

INSERT INTO [Lookup].[EntryOrExitPoint] (Name, CountryId)
VALUES	('Enschede', @HOLLAND),
		('Europoort', @HOLLAND),
		('Rotterdam', @HOLLAND)