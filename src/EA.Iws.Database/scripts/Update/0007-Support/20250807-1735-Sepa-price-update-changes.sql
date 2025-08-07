--11 = Single ship
INSERT INTO [Lookup].[PricingFixedFee] ([Id], [CompetentAuthority], [WasteComponentTypeId], [WasteCategoryTypeId], [Price], [ValidFrom])
	VALUES ('43A335BC-2618-4D7B-81EE-9B3CE7741CB6', @CompetentAuthority, null, 11, 10151, @validFrom);

UPDATE [Lookup].[CompetentAuthority] SET [Name] ='Scottish Environment Protection Agency' WHERE Abbreviation='SEPA' AND Code='GB02';