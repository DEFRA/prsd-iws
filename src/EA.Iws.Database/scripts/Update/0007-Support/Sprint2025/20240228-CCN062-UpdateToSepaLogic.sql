--Add New SEPA Category Charge
DECLARE @validFrom nvarchar(100);
SET @validFrom = (SELECT [Value] from [Lookup].[SystemSettings] where Id = 2);

INSERT INTO [Lookup].[PricingFixedFee] ([Id], [CompetentAuthority], [WasteComponentTypeId], [WasteCategoryTypeId], [Price], [ValidFrom])
	VALUES 
		('D39271D5-381C-43C0-A137-A190DB98F1AF', 2, null, 11, 9080, @validFrom)