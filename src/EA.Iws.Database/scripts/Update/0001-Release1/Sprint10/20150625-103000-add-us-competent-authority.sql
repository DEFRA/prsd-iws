PRINT 'Inserting US Competent Authority'

DECLARE @USA UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM [Lookup].[Country] WHERE Name = 'United States');

INSERT INTO [Lookup].[CompetentAuthority]
(Name, Abbreviation, Code, CountryId)
VALUES
('US Environmental Protection Agency', 'EPA', '2254A', @USA);

INSERT INTO [Lookup].[EntryOrExitPoint]
(Name, CountryId)
VALUES
('Boston', @USA),
('New York', @USA),
('New Orleans', @USA),
('Houston', @USA);

GO