
DECLARE @EAID UNIQUEIDENTIFIER = (SELECT ID FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'EA' AND IsTransitAuthority = 0)
DECLARE @SEPAID UNIQUEIDENTIFIER = (SELECT ID FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'SEPA' AND IsTransitAuthority = 0)
DECLARE @NRWID UNIQUEIDENTIFIER = (SELECT ID FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'NRW' AND IsTransitAuthority = 0)
DECLARE @NIEAID UNIQUEIDENTIFIER = (SELECT ID FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'NIEA' AND IsTransitAuthority = 0)

DECLARE @EA_UKCA_ID INT = (SELECT ID FROM [Lookup].[UnitedKingdomCompetentAuthority] WHERE CompetentAuthorityId = @EAID)
DECLARE @SEPA_UKCA_ID INT = (SELECT ID FROM [Lookup].[UnitedKingdomCompetentAuthority] WHERE CompetentAuthorityId = @SEPAID)
DECLARE @NRW_UKCA_ID INT = (SELECT ID FROM [Lookup].[UnitedKingdomCompetentAuthority] WHERE CompetentAuthorityId = @NRWID)

INSERT INTO [Lookup].[IntraCountryExportAllowed]
([ExportCompetentAuthority], [ImportCompetentAuthorityID])
VALUES
(@EA_UKCA_ID,@NIEAID),
(@NRW_UKCA_ID,@NIEAID),
(@SEPA_UKCA_ID,@NIEAID)
