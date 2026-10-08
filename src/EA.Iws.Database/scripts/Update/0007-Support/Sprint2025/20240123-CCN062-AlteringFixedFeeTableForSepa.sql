PRINT N'Altering [Lookup].[PricingFixedFee]...';

GO
--Add column for determining which CompetentAuthority the Pricing Fixed Fee is for
ALTER TABLE [Lookup].[PricingFixedFee] ADD [CompetentAuthority] INT NOT NULL CONSTRAINT [DF_CompetentAuthority] DEFAULT '1';

GO
ALTER TABLE [Lookup].[PricingFixedFee] DROP CONSTRAINT [DF_CompetentAuthority];

DECLARE @validFrom nvarchar(100);
SET @validFrom = (SELECT [Value] from [Lookup].[SystemSettings] where Id = 2);

INSERT INTO [Lookup].[PricingFixedFee] ([Id], [CompetentAuthority], [WasteComponentTypeId], [WasteCategoryTypeId], [Price], [ValidFrom])
	VALUES 
		('AA5D920C-2E15-4EA1-A68A-401DDD50D3CF', 2, null, 12, 9080, @validFrom)

GO
