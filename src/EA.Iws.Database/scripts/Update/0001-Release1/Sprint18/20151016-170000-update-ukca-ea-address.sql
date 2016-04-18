UPDATE [Lookup].[UnitedKingdomCompetentAuthority]
SET [BusinessUnit]='International Waste Shipments Service', [Address1] = 'Knutsford Road'
WHERE [CompetentAuthorityId] = (SELECT TOP 1 [Id] FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'EA');
GO