DECLARE @EAID UNIQUEIDENTIFIER = (SELECT ID FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'EA' AND IsTransitAuthority = 0)
DECLARE @SEPAID UNIQUEIDENTIFIER = (SELECT ID FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'SEPA' AND IsTransitAuthority = 0)
DECLARE @NRWID UNIQUEIDENTIFIER = (SELECT ID FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'NRW' AND IsTransitAuthority = 0)
DECLARE @NIEAID UNIQUEIDENTIFIER = (SELECT ID FROM [Lookup].[CompetentAuthority] WHERE Abbreviation = 'NIEA' AND IsTransitAuthority = 0)

INSERT INTO [Lookup].[IntraCountryExportAllowed]
(ExportCompetentAuthorityID, ImportCompetentAuthorityID)
VALUES
(@SEPAID,@NIEAID),
(@NRWID,@NIEAID),
(@EAID,@NIEAID)
