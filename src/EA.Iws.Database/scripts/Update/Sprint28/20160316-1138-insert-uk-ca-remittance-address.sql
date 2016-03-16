-- Set SEPA address
UPDATE [Lookup].[UnitedKingdomCompetentAuthority]
SET [RemittancePostalAddress]='Scottish Environment Protection Agency, Producer Compliance and Waste Shipment Unit, Strathallan House, The Castle Business Park, Stirling, FK9 4TZ'
WHERE [Id] = 2

-- Set NIEA address
UPDATE [Lookup].[UnitedKingdomCompetentAuthority]
SET [RemittancePostalAddress]='Northern Ireland Environment Agency, TFS Section, 1st Floor Klondyke Building, Cromac Avenue, Gasworks Business Park, Malone Lower, Belfast, BT7 2JA'
WHERE [Id] = 3

-- Set NRW address
UPDATE [Lookup].[UnitedKingdomCompetentAuthority]
SET [RemittancePostalAddress]='Natural Resources Wales, Waste Shipment Unit, Plas-yr-Afon/Rivers House, St. Mellons Business Park, Cardiff, CF3 0EY'
WHERE [Id] = 4

-- Set [RemittancePostalAddress] as non-nullable
ALTER TABLE [Lookup].[UnitedKingdomCompetentAuthority] ALTER COLUMN [RemittancePostalAddress] NVARCHAR(4000) NOT NULL