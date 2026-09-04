UPDATE [Lookup].[UnitedKingdomCompetentAuthority]
SET [RemittancePostalAddress]='Scottish Environment Protection Agency, Producer Compliance and Waste Shipment Unit, Angus Smith Building, 6 Parklands Avenue, Eurocentral, North Lanarkshire, ML1 4WQ.'
WHERE [CompetentAuthorityId] = (SELECT TOP 1 [Id] FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'SEPA');
GO